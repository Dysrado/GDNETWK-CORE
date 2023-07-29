using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class PickupDetection : NetworkBehaviour
{

    //Temporary Placeholder for the weapons
    private Magnum magnum;
    private SMG smg;
    private Sniper sniper;
    private Rifle rifle;
    private Shotgun shotgun;



    // Start is called before the first frame update
    void Start()
    {
        magnum = this.gameObject.GetComponent<Magnum>();
        smg = this.gameObject.GetComponent<SMG>();
        sniper = this.gameObject.GetComponent<Sniper>();
        rifle = this.gameObject.GetComponent<Rifle>();
        shotgun = this.gameObject.GetComponent<Shotgun>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Pickup Detection Need help for detecting and removing multiplayer
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Debug.LogWarning("Collided with a pickup");
            //RequestDeletePickupServerRpc(other.gameObject);

            if (magnum.isActiveAndEnabled)
                magnum.RecoverAmmo();

            else if (smg.isActiveAndEnabled)
                smg.RecoverAmmo();

            else if (sniper.isActiveAndEnabled)
                sniper.RecoverAmmo();

            else if (rifle.isActiveAndEnabled)
                rifle.RecoverAmmo();

            else if (shotgun.isActiveAndEnabled)
                shotgun.RecoverAmmo();

            other.GetComponent<NetworkObject>().Despawn(); 
           // RequestDeletePickupServerRpc(other.GetComponent<NetworkObject>());
        }
    }

    

   

}
