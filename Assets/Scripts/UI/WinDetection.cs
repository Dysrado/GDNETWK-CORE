using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

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


    public void OnWin(FixedString128Bytes playerWinner)
    {
        if (IsHost)
        {
            //Instantiate the win
            GameObject winCopy = Instantiate(winPrefab, parentCanvas.transform, false);
            winCopy.GetComponent<WinBehaviour>().SetName(playerWinner);
            winCopy.GetComponent<NetworkObject>().Spawn(true);
            winCopy.GetComponent<WinBehaviour>().SetName(playerWinner);
            winCopy.transform.parent = parentCanvas.transform;
          
        }
    }
}
