using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponClass : MonoBehaviour
{
    enum WeaponType
    {
        Magnum,
        SMG,
        Sniper
    }

    int ClipSize,ReserveAmmo,DMG;
    float Lifetime, ReloadSpeed, FireRate, ProjectileSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Fire()
    {

    }
}
