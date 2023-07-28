using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class TestRelay : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject MainLobbyUI;
    [SerializeField] GameObject GameMap;
    public static TestRelay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private  void Start()
    {
        //await UnityServices.InitializeAsync();

        //AuthenticationService.Instance.SignedIn += () => {
        //    Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        //};

        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    public async Task<string> CreateRelay()
    {
        try { 
           Allocation allocation =  await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            RelayServerData relayServerData = new RelayServerData(allocation, "udp");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            return joinCode;
        } catch (RelayServiceException e) 
        {
            Debug.Log(e);
            return null;
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            MainLobbyUI.SetActive(false);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);


            RelayServerData relayServerData = new RelayServerData(joinAllocation, "udp");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            GameMap.SetActive(true);

        } catch(RelayServiceException e) { 
            Debug.Log(e); }
    }
}
