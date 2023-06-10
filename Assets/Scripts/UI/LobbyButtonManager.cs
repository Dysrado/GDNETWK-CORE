using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LobbyButtonManager : MonoBehaviour
{
    public void OnHost()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.StartHost();
            this.gameObject.SetActive(false);
        }
    }

    public void OnJoin()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.StartClient();
            this.gameObject.SetActive(false);
        }
    }
}
