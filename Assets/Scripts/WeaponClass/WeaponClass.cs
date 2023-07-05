using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponClass : NetworkBehaviour
{

    [SerializeField] protected string WeaponName;
    private Vector3 mousePos;
    private Camera mainCamera;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float spawnOffset = 1.0f;
    protected bool isReloading;

    // Unique Gun Variables
    [SerializeField] protected float projectileSpeed = 3.0f;
    [SerializeField] protected int clipSize; // max size of current ammo
    [SerializeField] protected int reserveAmmo = 100;
    [SerializeField] protected int currentAmmo = 10;
    [SerializeField] protected float reloadSpeed = 1.0f;
    [SerializeField] protected float bulletLifetime = 0.6f;
    [SerializeField] protected float fireRate = 0.6f;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        reserveAmmo = 100;
        clipSize = 10;
        currentAmmo = clipSize;
        isReloading = false;
    }

    // Always call this on the update function for aiming 
    protected virtual void SetAim()
    {
        // Get world space mouse position
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            mousePos = hit.point;
        }
    }

    public virtual void Fire()
    {
        
        // Get direction towards mouse
        Vector3 playerToMouse;
        playerToMouse = mousePos - transform.position;
        playerToMouse = playerToMouse.normalized;

        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position + (new Vector3(playerToMouse.x, 0, playerToMouse.z) * spawnOffset), bulletPrefab.transform.rotation);
        Rigidbody bulletRb;
        bulletRb = bullet.GetComponent<Rigidbody>();
        // Apply force to bullet
        bulletRb.AddForce(playerToMouse * projectileSpeed, ForceMode.Impulse);
        Debug.Log("Shoot!");


        Destroy(bullet, bulletLifetime); // This can be optimized

        currentAmmo--; // Reduce clip by 1
                       // Automatic Reload, can remove if not needed
        if (currentAmmo <= 0)
            StartCoroutine(ReloadGun());
    }

    public virtual void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo != clipSize)
        {
            isReloading = true;
            StartCoroutine(ReloadGun());
        }
    }

    protected virtual IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(reloadSpeed);
        reserveAmmo -= clipSize;
        if (reserveAmmo >= 0)
        {
            currentAmmo = clipSize;
        }
        isReloading = false;
    }

    [ServerRpc]
    protected void RequestFireServerRpc()
    {
        ExecuteFireClientRpc();
    }

    [ClientRpc]
    protected void ExecuteFireClientRpc()
    {
        Fire();
    }
}
