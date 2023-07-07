using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickupDetection : NetworkBehaviour
{

    //Temporary Placeholder for the weapons
    private Magnum magnum;
    private SMG smg;
    private Sniper sniper;



    // Start is called before the first frame update
    void Start()
    {
        magnum = this.gameObject.GetComponent<Magnum>();
        smg = this.gameObject.GetComponent<SMG>();
        sniper = this.gameObject.GetComponent<Sniper>();

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
            Destroy(other.gameObject);
        }
    }

    //[ServerRpc]
    //protected void RequestDeletePickupServerRpc(GameObject pickup)
    //{
    //    ExecuteClientDeletePickupClientRpc(pickup);
    //}

    //[ClientRpc]
    //protected void ExecuteClientDeletePickupClientRpc(GameObject pickup)
    //{
    //    if (!IsOwner)
    //        Destroy(pickup);
    //}

}
