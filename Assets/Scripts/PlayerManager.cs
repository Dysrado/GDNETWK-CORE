using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private Magnum magnum;
    [SerializeField] private SMG smg;
    [SerializeField] private Sniper sniper;
    [SerializeField] private string username;
    [SerializeField] private int playerID;

    private WeaponClass activeWeapon;



    // Start is called before the first frame update
    void Start()
    {
        magnum = GetComponent<Magnum>();
        smg = GetComponent<SMG>();
        sniper = GetComponent<Sniper>();

        // Default Gun is Magnum
        smg.enabled = false;
        sniper.enabled = false;
        magnum.enabled = true;

        activeWeapon = (WeaponClass)magnum;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch Weapons 
        if(Input.GetKeyDown(KeyCode.Alpha1)) // Equip Magnum
        {
            smg.enabled = false;
            sniper.enabled = false;
            magnum.enabled = true;

            activeWeapon = (WeaponClass)magnum;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) // Equip SMG
        {
            magnum.enabled = false;
            sniper.enabled = false;
            smg.enabled = true;

            activeWeapon = (WeaponClass)smg;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Equip Sniper
        {
            magnum.enabled = false;
            smg.enabled = false;
            sniper.enabled = true;

            activeWeapon = (WeaponClass)sniper;
        }
    }

    //Bullet Detection
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullets"))
        {
            //Debug.LogWarning("Collided with a bullet");

            BulletID bulletId = collision.gameObject.GetComponent<BulletID>();
            //Debug.LogWarning($"Got Hit by {bulletId.GetOwnerId()}");


            /*
             * TODO:  
             *  1) Add parameters/component to the bullet for much damage can it take.
             */

            CheckHealth(bulletId.GetOwnerId(), 5);
        }
        
    }

    
    private void CheckHealth (int hitId, int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            int ownerId = this.gameObject.GetComponent<PlayerNetworkV2>().GetNetworkID();
            GamaManager.Instance.Killed(ownerId, hitId);
        }
    }

    public int GetCurrentAmmo()
    {
        return activeWeapon.currentAmmo;
    }

    public int GetReserveAmmo()
    {
        return activeWeapon.reserveAmmo;
    }

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
        return username;
    }

    public void SetInfo(int playerId)
    {
        this.playerID = playerId;
        
    }

    public void SetUserName(string playerUsername)
    {
        this.username = playerUsername;

        Debug.LogError(playerUsername);
    }
}
