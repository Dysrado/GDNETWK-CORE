using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Magnum : WeaponClass
{

    // Start is called before the first frame update
    void Awake()
    {
        projectileSpeed = 18.0f;
        reloadSpeed = 1.0f;
        bulletLifetime = 1.6f;
        reserveAmmo = 60;
        clipSize = 6;
        fireRate = 0.6f;
        currentAmmo = clipSize;
        name = "Magnum";
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
