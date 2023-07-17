using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEditor.PackageManager;


public class WeaponClass : NetworkBehaviour
{

    [SerializeField] protected string WeaponName;
    private Vector3 mousePos;
    private Camera mainCamera;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float spawnOffset = 1.0f;
    protected bool isReloading;
    protected bool canFire;

    // Unique Gun Variables - gonna make public for now for testing
    public float projectileSpeed;
    public int clipSize; // max size of current ammo
    public int reserveAmmo;
    public int currentAmmo;
    public float reloadSpeed;
    public float bulletLifetime;
    public float fireRate;

    /*Additional Paramters for the Weapon Class - Kevin*/
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        canFire = true;
        isReloading = false;
    }

    // Always call this on the update function for aiming 
    protected virtual Vector3 SetAim()
    {
        // Get world space mouse position
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            mousePos = hit.point;
        }
        // Get direction towards mouse
        Vector3 playerToMouse;
        playerToMouse = mousePos - transform.position;
        playerToMouse = playerToMouse.normalized;

        return playerToMouse;
    }

    public virtual void Fire(Vector3 playerToMouse)
    {
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position + (new Vector3(playerToMouse.x, 0, playerToMouse.z) * spawnOffset), bulletPrefab.transform.rotation);
        
        bullet.AddComponent<BulletID>().Initialize((int)OwnerClientId);
       
        Rigidbody bulletRb;
        bulletRb = bullet.GetComponent<Rigidbody>();
        // Apply force to bullet
        bulletRb.AddForce(playerToMouse * projectileSpeed, ForceMode.Impulse);
        Debug.Log("Shoot!");

        
        Destroy(bullet, bulletLifetime); // This can be optimized

        currentAmmo--; // Reduce clip by 1
                       // Automatic Reload, can remove if not needed
        if (currentAmmo <= 0 && reserveAmmo - clipSize >= 0)
            StartCoroutine(ReloadGun());
        Debug.Log("Current Ammo: " + currentAmmo);
        StartCoroutine(WaitBetweenShots());
    }

    public virtual void Reload()
    {
        isReloading = true;
        StartCoroutine(ReloadGun());
    }

    //Need to replace to be virtual function. Each weapon have their definition how to add bullets.
    public void RecoverAmmo()
    {
        reserveAmmo += clipSize * 2;
    }

    public virtual IEnumerator WaitBetweenShots()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }

    protected virtual IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(reloadSpeed);
        if(reserveAmmo - clipSize >= 0)
        {
            reserveAmmo -= clipSize;
            currentAmmo = clipSize;
        }
        isReloading = false;
    }

    [ServerRpc]
    protected void RequestFireServerRpc(Vector3 playerToMouse)
    {
        ExecuteFireClientRpc(playerToMouse);
    }

    [ClientRpc]
    protected void ExecuteFireClientRpc(Vector3 playerToMouse)
    {
        if(!IsOwner)
            Fire(playerToMouse);
    }
}
