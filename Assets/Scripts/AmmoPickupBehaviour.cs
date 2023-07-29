using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AmmoPickupBehaviour : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Destroy or Despawn Itself upon hitting somethn other than bullet or player
        string collisionTag = collision.gameObject.tag;

        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Bullets")) 
        {
            Debug.LogWarning("Collided with something");
            if(IsServer)
                this.gameObject.GetComponent<NetworkObject>().Despawn();
        }
    }


    //private void OnDestroy()
    //{
    //    AmmoSpawnManager ammoSpawnMCopy = GameObject.FindObjectOfType<AmmoSpawnManager>();
    //    Debug.Log("Destroy Powerup");
    //}

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            AmmoSpawnManager ammoSpawnMCopy = GameObject.FindObjectOfType<AmmoSpawnManager>();
            ammoSpawnMCopy.AmmoConsumed();
        }
        base.OnNetworkDespawn();
    }
}
