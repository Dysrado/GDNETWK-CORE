using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AmmoSpawnManager : NetworkBehaviour
{
    [Header("Spawn Parameters")]
    [SerializeField] private float length;
    [SerializeField] private float width;
    [SerializeField] private Vector3 center;
    [SerializeField] private float intervals = 5.0f; //Default


    [Header("Spawn Parameters")]
    [Range(-10.0f, 10.0f)]
    [SerializeField] private float heightOffset;
    [Range(-50.0f, 50.0f)]
    [SerializeField] private float xOffset;
    [Range(-50.0f, 50.0f)]
    [SerializeField] private float zOffset;
    //Reference
    [Header("Object Reference")]
    [SerializeField] private GameObject centerRef;
    [SerializeField] private GameObject ammoSpawnPrefab;

    //Internal Tracking
    private bool hasStarted = false;
    private int maxAmmoPresent = 30;
    private int ammoPresent = 0;

    private float elapseTime = 0.0f;
    private bool hasSuccesfullySpawn = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(elapseTime > intervals)
        {
            OnRandomSpawn();
        }

        elapseTime += Time.deltaTime;
    }

    

    public void OnRandomSpawn()
    {
        
        if (hasStarted && !hasSuccesfullySpawn && maxAmmoPresent > ammoPresent)
        {
            //Randomize the spawn for the object
            Vector3 spawnLoc = new Vector3(
                center.x + Random.Range(-length + xOffset, length + xOffset),
                center.y + heightOffset,
                center.z + Random.Range(-width + zOffset, width + zOffset)
                );

            //Create a couroutine that will check if the spawn is successfull
            enabled = IsServer;
            if(enabled)
                StartCoroutine( TrySpawning(spawnLoc));

            //If successful wait for another one to spawn at x-seconds

            //Repeat
        }
        //Debug.LogError("On Spawn Call");

    }

    IEnumerator TrySpawning(Vector3 spawnLoc)
    {
       

        GameObject ammoCopy = Instantiate(ammoSpawnPrefab, spawnLoc, (Quaternion)ammoSpawnPrefab.transform.rotation);
        ammoCopy.GetComponent<NetworkObject>().Spawn(); 
        ammoPresent++;
        hasSuccesfullySpawn = true;

        yield return new WaitForSeconds(.1f);
        elapseTime = 0.0f;
        hasSuccesfullySpawn = false;
        //Temp Fix
    }

    public void AmmoConsumed()
    {
        ammoPresent--;
    }


    public void EndGame() {
        hasStarted = false;
    }

    public void StartGame() {
        ammoPresent = 0;
        hasStarted = true;
    }


    /*----Debugging Section----*/

    private void OnDrawGizmosSelected()
    {
        //Debugging tool for drawing the map == Disable this whenever not needed
        
            //Declare the Center
            center = centerRef.transform.position;

            //Find the edge of the rectangle given the length and width
            Vector3 P4 = new Vector3(center.x + length + xOffset, 
                center.y + heightOffset, center.z + width + zOffset); //Q1
            Vector3 P3 = new Vector3(center.x - length + xOffset, 
                center.y + heightOffset, center.z + width + zOffset); //Q2
            Vector3 P1 = new Vector3(center.x - length + xOffset, 
                center.y + heightOffset, center.z - width + zOffset); //Q3
            Vector3 P2 = new Vector3(center.x + length + xOffset, 
                center.y + heightOffset, center.z - width + zOffset); //Q4



            //Draw the line to visualize the area.
            Gizmos.color = Color.red;

            Gizmos.DrawLine(P4, P3);
            Gizmos.DrawLine(P3, P1);
            Gizmos.DrawLine(P1, P2);
            Gizmos.DrawLine(P2, P4);

           
    }
}
