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
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) // Equip SMG
        {
            magnum.enabled = false;
            sniper.enabled = false;
            smg.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Equip Sniper
        {
            magnum.enabled = false;
            smg.enabled = false;
            sniper.enabled = true;
        }
    }
}
