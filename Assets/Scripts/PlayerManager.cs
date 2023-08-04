using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Magnum magnum;
    [SerializeField] private SMG smg;
    [SerializeField] private Sniper sniper;
    [SerializeField] private Rifle rifle;
    [SerializeField] private Shotgun shotgun;

    //[SerializeField] private string username;

    [Header("Player Information")]
    [SerializeField] private int playerID;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private int killCount;
    [SerializeField] private int score;

    private WeaponClass activeWeapon;
    private NameTagBehaviour nameTag;

    // Respawn Variables
    public GameObject respawnPoint = null;
    public float respawnTime = 3.0f;

    // Other Player Variables
    GameObject[] players;
    public bool isDead;
    public CapsuleCollider capsuleCollider;
    public GameObject bodyRenderer;

    [SerializeField] private GameObject[] respawnPoints;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        players = new GameObject[4]; // assuming 4 people

        magnum = GetComponent<Magnum>();
        smg = GetComponent<SMG>();
        sniper = GetComponent<Sniper>();
        rifle = GetComponent<Rifle>();
        shotgun = GetComponent<Shotgun>();

        // Default Gun is Magnum
        rifle.enabled = false;
        shotgun.enabled = false;
        smg.enabled = false;
        sniper.enabled = false;
        magnum.enabled = true;

        activeWeapon = (WeaponClass)magnum;

        //Default Stats
        score = 0;
        killCount = 0;

        // Is Dead
        isDead = false;
        respawnPoints = GameObject.FindGameObjectsWithTag("RespawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        // Set Respawn Points
        if(respawnPoint == null)
        {

            if(respawnPoints.Length > 0)
            {
                //if (playerID == 0)
                //{
                //    respawnPoint = respawnPoints[0];
                //}
                //else if (playerID == 1)
                //{
                //    respawnPoint = respawnPoints[1];
                //}
                //else if (playerID == 2)
                //{
                //    respawnPoint = respawnPoints[2];
                //}
                //else if (playerID == 3)
                //{
                //    respawnPoint = respawnPoints[3];
                //}
                //else
                //{
                //    Debug.Log("5 / 4 Players, Not Enough Spawn Points.");
                //}
                
            }
            
        }

        // Keep Track of Players
        players = GameObject.FindGameObjectsWithTag("Player");

        /*
        // Switch Weapons 
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Equip Magnum
        {
            shotgun.enabled = false;
            rifle.enabled = false;
            smg.enabled = false;
            sniper.enabled = false;
            magnum.enabled = true;

            activeWeapon = (WeaponClass)magnum;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) // Equip SMG
        {
            shotgun.enabled = false;
            rifle.enabled = false;
            magnum.enabled = false;
            sniper.enabled = false;
            smg.enabled = true;

            activeWeapon = (WeaponClass)smg;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Equip Sniper
        {
            shotgun.enabled = false;
            rifle.enabled = false;
            magnum.enabled = false;
            smg.enabled = false;
            sniper.enabled = true;

            activeWeapon = (WeaponClass)sniper;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Equip Rifle
        {
            shotgun.enabled = false;
            magnum.enabled = false;
            smg.enabled = false;
            sniper.enabled = false;
            rifle.enabled = true;

            activeWeapon = (WeaponClass)rifle;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) // Equip Shotgun
        {
            magnum.enabled = false;
            smg.enabled = false;
            sniper.enabled = false;
            rifle.enabled = false;
            shotgun.enabled = true;

            activeWeapon = (WeaponClass)shotgun;
        }
        */
        
    }

    //Bullet Detection
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullets"))
        {
            //Debug.LogWarning("Collided with a bullet");
            BulletInfo bulletId = collision.gameObject.GetComponent<BulletInfo>();

            //Debug.LogWarning($"Got Hit by {bulletId.GetOwnerId()}");
            if(bulletId.GetOwnerId() != playerID)
            {
                CheckHealth(bulletId.GetOwnerId(), bulletId.GetBulletDamage());
            }
        }
    }

  
    private void CheckHealth(int shooterID, int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            isDead = true;
            bodyRenderer.SetActive(false);
            capsuleCollider.enabled = false;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
            int ownerId = this.gameObject.GetComponent<PlayerNetworkV2>().GetNetworkID();
            GamaManager.Instance.Killed(ownerId, shooterID);
            StartCoroutine(RespawnPlayer());  
        }
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnTime);
        currentHealth = 100;
        if (respawnPoints.Length == 0)
        {
            respawnPoints = GameObject.FindGameObjectsWithTag("RespawnPoint");
        }
        int random = UnityEngine.Random.Range(0, respawnPoints.Length - 1);
        respawnPoint = respawnPoints[random];
        gameObject.transform.position = respawnPoint.transform.position; // Set Player Position to Spawn Point
        isDead = false;
        bodyRenderer.SetActive(true);
        capsuleCollider.enabled = true;
        
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

    }

    public void ResetStats()
    {
        //TODO: Refresh the total amount of bullet or reload scene


        //Base Stats
        score = 0;
        currentHealth = maxHealth;
    }

    /* Weapon Upgrade */

    public void UpgradeWeapon(int killrequireMents)
    {
        int tier = this.score / killrequireMents;
        switch (tier)
        {
            case 0:
                shotgun.enabled = false;
                rifle.enabled = false;
                smg.enabled = false;
                sniper.enabled = false;
                magnum.enabled = true;

                activeWeapon = (WeaponClass)magnum;
                break;

            case 1:
                shotgun.enabled = false;
                rifle.enabled = false;
                magnum.enabled = false;
                sniper.enabled = false;
                smg.enabled = true;

                activeWeapon = (WeaponClass)smg;
                break;

            case 2:
                shotgun.enabled = false;
                rifle.enabled = false;
                magnum.enabled = false;
                smg.enabled = false;
                sniper.enabled = true;

                activeWeapon = (WeaponClass)sniper;
                break;
            case 3:
                shotgun.enabled = false;
                magnum.enabled = false;
                smg.enabled = false;
                sniper.enabled = false;
                rifle.enabled = true;

                activeWeapon = (WeaponClass)rifle;
                break;
            case 4:
                magnum.enabled = false;
                smg.enabled = false;
                sniper.enabled = false;
                rifle.enabled = false;
                shotgun.enabled = true;

                activeWeapon = (WeaponClass)shotgun;
                break;
        }
    }


    public void OnKill()
    {
        killCount++;
        score++;
    }

    /*Extra Function - for meelee kill*/
    public void OnMeeleDeath()
    {

    }


    /*Weapon Section*/

    public WeaponClass GetActiveWeapon()
    {
        return activeWeapon;
    }

    public int GetCurrentAmmo()
    {
        return activeWeapon.currentAmmo;
    }

    public int GetReserveAmmo()
    {
        return activeWeapon.reserveAmmo;
    }

    public string GetWeaponName()
    {
        return activeWeapon.name;
    }

    /*CharacterSection*/
    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetPlayerID()
    {
        return playerID;
    }

    public string GetPlayerUsername()
    {
        return nameTag.GetName();
    }
    
    public int GetScore()
    {
        return this.score;
    }
    public void SetInfo(int playerId, NameTagBehaviour nameTag)
    {
        this.playerID = playerId;
        this.nameTag = nameTag;
        
    }

    
}
