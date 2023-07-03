using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float spawnOffset = 1.0f;
    private GameObject bullet;
    private bool isReloading;

    // Unique Gun Variables
    [SerializeField] private float projectileSpeed = 3.0f;
    [SerializeField] private int clipSize; // max size of current ammo
    [SerializeField] private int reserveAmmo;
    [SerializeField] private int currentAmmo;
    [SerializeField] private float reloadSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        reserveAmmo = 100;
        clipSize = 10;
        currentAmmo = clipSize;
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get world space mouse position
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            mousePos = hit.point;
        }
        Debug.Log(mousePos);

        // Fire bullet
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !isReloading)
        {
            // Get direction towards mouse
            Vector3 playerToMouse;
            playerToMouse = mousePos - transform.position;
            playerToMouse = playerToMouse.normalized;

            // Spawn bullet
            bullet = Instantiate(bulletPrefab, transform.position + (new Vector3(playerToMouse.x, 0, playerToMouse.z) * spawnOffset), bulletPrefab.transform.rotation);
            Rigidbody bulletRb;
            bulletRb = bullet.GetComponent<Rigidbody>();
            // Apply force to bullet
            bulletRb.AddForce(playerToMouse * projectileSpeed, ForceMode.Impulse);
            Debug.Log("Shoot!");
            currentAmmo--; // Reduce clip by 1
            // Automatic Reload, can remove if not needed
            if(currentAmmo <= 0)
                StartCoroutine(ReloadGun());
        }

        // Manual Reload 
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo != clipSize)
        {
            isReloading = true;
            StartCoroutine(ReloadGun());   
        }


    }

    IEnumerator ReloadGun()
    {
        yield return new WaitForSeconds(reloadSpeed);
        reserveAmmo -= clipSize;
        if (reserveAmmo >= 0)
        {
            currentAmmo = clipSize;
        }
        isReloading = false;
    }
}
