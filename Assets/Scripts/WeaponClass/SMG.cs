using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class SMG : WeaponClass
{

    // Start is called before the first frame update
    void Awake()
    {
        damage = 15; // 7 Shots to Kill
        projectileSpeed = 13.0f;
        reloadSpeed = 1.6f;
        bulletLifetime = 1.2f;
        reserveAmmo = 225;
        clipSize = 15;
        fireRate = 0.2f;
        currentAmmo = clipSize;
        name = "SMG";
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        Vector3 dir = SetAim();
        // Fire bullet
        if (Input.GetMouseButton(0) && currentAmmo > 0 && !isReloading && canFire)
        {
            RequestFireServerRpc(dir);
            Fire(dir);
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != clipSize)
        {
            Reload();
        }
    }

}
