using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamaManager : MonoBehaviour
{
    [Header("UI Placeholder")]
    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI timerTxt;
    [SerializeField] private TextMeshProUGUI ammoTxt;

    [Header("AnnnouncementSection")]
    [SerializeField] private GameObject announcementHolder;
    [SerializeField] private GameObject prefabAnnouncement;



    private PlayerManager playerOwner;
    private bool ownerPresent = false;
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
        if(timer < timeElapse)
        {
            CheckPlayerList();
            UpdateStats();
            timeElapse = 0.0f;
        }

        timeElapse += Time.deltaTime;
        
    }

    public void Killed(int ownerId, int killedId)
    {
        Debug.Log($"Player: {playerInfo[ownerId].GetPlayerUsername()} was killed by {playerInfo[killedId].GetPlayerUsername()}");
        GameObject killAnnouncement = prefabAnnouncement;
        Instantiate(killAnnouncement, announcementHolder.transform);

        killAnnouncement.GetComponent<KillAnnouncementBehaviour>().InsertText($"Player: {playerInfo[ownerId].GetPlayerUsername()} was killed by {playerInfo[killedId].GetPlayerUsername()}");
    }

    private void CheckPlayerList()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");


        foreach (GameObject player in players)
        {
            if (player.GetComponent<PlayerNetworkV2>().isActiveAndEnabled)
            {
                playerOwner = player.GetComponent<PlayerManager>();
                ownerPresent = true;
            }
        }

        if (ownerPresent)
        {
            foreach (GameObject player in players)
            {
                PlayerManager copy = player.GetComponent<PlayerManager>();
                playerInfo.TryAdd(copy.GetPlayerID(), copy);    
            }
        }

    }

    public void UpdateStats()
    {
        if (ownerPresent)
        {
            healthTxt.text = playerOwner.GetHealth().ToString();
            ammoTxt.text = $" {playerOwner.GetWeaponName()} <br> {playerOwner.GetCurrentAmmo()} | {playerOwner.GetReserveAmmo()}";
        }

    }

}
