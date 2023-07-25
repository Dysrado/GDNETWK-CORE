using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIEvents : MonoBehaviour
{
    [SerializeField] TMP_Text[] playerTextUI;
    private bool inLobby = false;
    public static LobbyUIEvents Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TestLobby.Instance.OnJoinedLobby += UpdateLobby_Event;
        TestLobby.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
    }
    private void UpdateLobby_Event(object sender, TestLobby.LobbyEventArgs e)
    {
        UpdateLobby();
    }

    private void UpdateLobby()
    {
        UpdateLobby(TestLobby.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby)
    {
        if (inLobby)
        {
            ClearLobby();
            int i = 0;
            foreach (Player player in lobby.Players)
            {
                playerTextUI[i].SetText(player.Data["PlayerName"].Value);
                i++;
                Debug.Log("Updated " + player.Data["PlayerName"].Value + "in text" + i);

            }
        }
        

    }

    public void ToggleLobby()
    {
        if (inLobby)
        {
            inLobby = false;
        }
        else
        {
            inLobby = true;
        }
    }
    private void ClearLobby()
    {
        for (int i = 0; i <3; i++)
            {
                playerTextUI[i].SetText("Empty");
            }
    }
}
