using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SampleNetBehav : MonoBehaviour
{
    //[ServerRpc]
    //public void PingServerRpc(int pingCount)
    //{

    //}
    //[ServerRpc(RequireOwnership = false)]
    //public void MyGlobalServerRpc(ulong clientId) // This is considered a bad practice (Not Recommended)
    //{
    //    if (NetworkManager.ConnectedClients.ContainsKey(clientId))
    //    {
    //        var client = NetworkManager.ConnectedClients[clientId];
    //        Do things for this client
    //    }
    //}

    //public override void OnNetworkSpawn()
    //{
    //    MyGlobalServerRpc(); // serverRpcParams will be filled in automatically
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void PlayerShootGunServerRpc(Vector3 lookWorldPosition, ServerRpcParams serverRpcParams = default)
    //{
    //    var clientId = serverRpcParams.Receive.SenderClientId;
    //    if (NetworkManager.ConnectedClients.ContainsKey(clientId))
    //    {
    //        var client = NetworkManager.ConnectedClients[clientId];
    //        var castRay = new Ray(client.PlayerObject.transform.position, lookWorldPosition);
    //        RaycastHit rayCastHit;
    //        if (Physics.Raycast(castRay, out rayCastHit, 100.0f))
    //        {
    //            Handle shooting something
    //        }
    //    }
    //}

}
