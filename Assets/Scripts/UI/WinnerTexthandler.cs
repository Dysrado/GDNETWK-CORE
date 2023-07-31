using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using Unity.Collections;

public class WinnerTexthandler : NetworkBehaviour
{
    [SerializeField] private GameObject WinnerScreenUI;

    public void WinScreen(FixedString128Bytes name)
    {
        GameObject winCopy = Instantiate(WinnerScreenUI, FindObjectOfType<Canvas>().transform, false);
        winCopy.GetComponent<WinBehaviour>().SetName(name);
        
        //winCopy.GetComponent<NetworkObject>().Spawn(true);
        Debug.Log("added screen");
        
    }
    public void Leave()
    {
        FindObjectOfType<TestLobby>().Disconnect();
    }

    [ServerRpc(RequireOwnership =false)]
    public void RequestWinScreenServerRPC(FixedString128Bytes name)
    {
        WinScreenClientRPC(name);
        WinScreen(name);
        Debug.Log("FireServer");
    }

    [ClientRpc]
    public void WinScreenClientRPC(FixedString128Bytes name)
    {
        if (!IsOwner)
        {
            WinScreen(name);
        }
    }


}
