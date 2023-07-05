using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Magnum : WeaponClass
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        SetAim();
        // Fire bullet
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !isReloading)
        {
            RequestFireServerRpc();
        }
        
        Reload();
    }

}
