using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GamaManager : MonoBehaviour
{
    [Header("UI Placeholder")]
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private TextMeshProUGUI ammoTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject EndScreenUI;



    [Header("AnnnouncementSection")]
    [SerializeField] private GameObject announcementHolder;
    [SerializeField] private GameObject prefabAnnouncement;

    //Change it into struct

    private PlayerManager playerOwner;
    [Header("Game Parameters")]
    [SerializeField] private int levelUpRequirement;
    [SerializeField] private int maxScore;
    [SerializeField] private int maxGameDuration;
    private float elapsedGameTime = 0;

    //Goals
    /*Start the round
     *  Check for Condition and only host would start the round
     * End Round
      */

    /*Hidden Parameters*/
    private bool ownerPresent = false;
    private bool isHost = false;
    private bool hasStarted = false;

    

    //Internal Clock
    private float timer = 0.1f;
    private float timeElapse = 10.0f;

    private Dictionary<int, PlayerManager> playerInfo;


    public static GamaManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playerInfo = new Dictionary<int, PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        //For every x-seconds check the number of players active in the server
        if (timer < timeElapse)
        {
            CheckPlayerList();
            UpdateStats();
            timeElapse = 0.0f;

            if (elapsedGameTime > maxGameDuration && hasStarted)
            {
                //Call a function to call the end game;
                EndGame();

                if(FindWinner() != null)
                    FindObjectOfType<WinnerTexthandler>().RequestWinScreenServerRPC(FindWinner().GetPlayerUsername());

            }

            else
            {
                CheckHost();
            }
          
        }
        if (hasStarted)
        {
            elapsedGameTime += Time.deltaTime;
        }

        timeElapse += Time.deltaTime;
        
    }

    //Internal Checking
    private void CheckHost()
    {
        if (ownerPresent)
        {
            //Under normal circumstances
            if (playerOwner.GetPlayerID() == 0)
                isHost = true;

            if (isHost && !hasStarted)
            {
                //Enable the start button
                GameObject[] players;
                players = GameObject.FindGameObjectsWithTag("Player");

                PreGameCheck(players.Length);

            }
        }
       
    }

    public void StartGame()
    {
        hasStarted = true;
        GameObject.FindObjectOfType<AmmoSpawnManager>().StartGame();
        //this.startBtn.SetActive(false);
    }

    public void EndGame()
    {
        hasStarted = false;
        GameObject.FindObjectOfType<AmmoSpawnManager>().EndGame();
        elapsedGameTime = 0.0f;
    }



    //Core Gameplay mechanics
    public void Killed(int ownerId, int killedId)
    {
        Debug.Log($"{playerInfo[ownerId].GetPlayerUsername()} was killed by {playerInfo[killedId].GetPlayerUsername()}");

        GameObject killAnnouncement = Instantiate(prefabAnnouncement, announcementHolder.transform); ;
        killAnnouncement.GetComponent<KillAnnouncementBehaviour>().InsertText($"Player: {playerInfo[ownerId].GetPlayerUsername()} was killed by {playerInfo[killedId].GetPlayerUsername()}");

        UpdateScore(killedId);
    }

    public void UpdateScore(int killerId)
    {
        playerInfo[killerId].OnKill();
        //Ill change this after at some point
        playerInfo[killerId].UpgradeWeapon(levelUpRequirement);

        if(playerInfo[killerId].GetScore() >= maxScore) //Detects if the player has won
        {
            EndGame();
            FindObjectOfType<WinnerTexthandler>().RequestWinScreenServerRPC(playerInfo[killerId].GetPlayerUsername());

            Debug.LogError("You win bro");
        }
    }

    
    //Player Updates Section
    private void CheckPlayerList()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        
        if (ownerPresent)
        {
            foreach (GameObject player in players)
            {
                PlayerManager copy = player.GetComponent<PlayerManager>();
                playerInfo.TryAdd(copy.GetPlayerID(), copy);    
            }
        }


        else
        {
            foreach (GameObject player in players)
            {
                if (player.GetComponent<PlayerNetworkV2>().isActiveAndEnabled)
                {
                    playerOwner = player.GetComponent<PlayerManager>();
                    ownerPresent = true;
                }
            }
        }

    }

    public void UpdateStats()
    {
        if (ownerPresent)
        {
            healthTxt.text = playerOwner.GetHealth().ToString();
            ammoTxt.text = $" {playerOwner.GetWeaponName()} <br> {playerOwner.GetCurrentAmmo()} | {playerOwner.GetReserveAmmo()}";
            scoreTxt.text = $"Kill Count: {playerOwner.GetScore()}";
        }

        if (hasStarted)
        {
            timerTxt.text = $"{(int)(maxGameDuration - elapsedGameTime)}s";
        }

    }

    private void ClearGameStats()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerManager>().ResetStats();
        }

    }

    private void PreGameCheck(int playerSize)
    {
        
        if(playerSize > 1 && isHost && !hasStarted)
        {
            //Insert Fucntion to enable the start button
            //startBtn.SetActive(true);
        }
    }

    public PlayerManager FindWinner()
    {
       
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");

        int highestScore = 0;
        int winIndex = 0;
        int counter = 0;

        foreach (GameObject player in players)
        {

            if(highestScore < player.GetComponent<PlayerManager>().GetScore())
            {
                highestScore = player.GetComponent<PlayerManager>().GetScore();
                winIndex = counter;
            }

            counter++;
            
        }
        
        if(counter == 0) {
            Debug.Log("No Player Found"); return null;
        }

        return playerInfo[winIndex];
    }

    private void SetEndScreen(PlayerManager player)
    {
      GameObject.FindObjectOfType<WinDetection>().OnWin(player.GetPlayerUsername());
       
        //winnerText.SetText(player.GetPlayerUsername());
        //Time.timeScale = 0.0f;
        
    }
    
}
