using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Sniper : WeaponClass
{

    // Start is called before the first frame update
    void Awake()
    {
        projectileSpeed = 35.0f;
        reloadSpeed = 3.0f;
        bulletLifetime = 3.0f;
        reserveAmmo = 10;
        clipSize = 5;
        fireRate = 1.4f;
        currentAmmo = clipSize;
        name = "Sniper";
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
