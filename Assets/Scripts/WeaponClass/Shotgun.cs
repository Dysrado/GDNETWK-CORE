using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Shotgun : WeaponClass
{

    // Start is called before the first frame update
    void Awake()
    {
        damage = 50; // 2 Shot to Kill
        projectileSpeed = 25.0f;
        reloadSpeed = 2.4f;
        bulletLifetime = 0.15f; 
        reserveAmmo = 45;
        clipSize = 15; // shots 3 at a time
        fireRate = 0.8f;
        currentAmmo = clipSize;
        name = "Shotgun";
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector3 dir = SetAim();
        // Fire bullet
        if (Input.GetMouseButton(0) && currentAmmo > 0 && !isReloading && canFire)
        {
            // Create 2 more projectiles to create cone
            Vector3 dir2 = Quaternion.Euler(0, 20, 0) * dir;
            Vector3 dir3 = Quaternion.Euler(0, -20, 0) * dir;

            RequestFireServerRpc(dir);
            Fire(dir);

            RequestFireServerRpc(dir2);
            Fire(dir2); 

            RequestFireServerRpc(dir3);
            Fire(dir3);
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != clipSize)
        {
            Reload();
        }
    }

}
