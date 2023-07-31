using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WinDetection : NetworkBehaviour
{
    [SerializeField] private GameObject parentCanvas;
    [SerializeField] private GameObject winPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnWin(string playerWinner)
    {
        if (IsHost)
        {
            //Instantiate the win
            GameObject winCopy = Instantiate(winPrefab, parentCanvas.transform, false);
            winCopy.GetComponent<NetworkObject>().Spawn();
            winCopy.GetComponent<WinBehaviour>().SetName(playerWinner);
            winCopy.transform.parent = parentCanvas.transform;
          
        }
    }
}
