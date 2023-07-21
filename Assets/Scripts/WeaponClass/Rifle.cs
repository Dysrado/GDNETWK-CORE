using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Rifle : WeaponClass
{

    // Start is called before the first frame update
    void Awake()
    {
        damage = 25; // 4 Shots to Kill
        projectileSpeed = 20.0f;
        reloadSpeed = 2.0f;
        bulletLifetime = 2.0f;
        reserveAmmo = 120;
        clipSize = 12;
        fireRate = 0.4f;
        currentAmmo = clipSize;
        name = "Rifle";
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
