using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Collections;

public class WinBehaviour : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(FixedString128Bytes name)
    {
        playerName.text = name.ToString();
    }

    public void Leave()
    {
        FindObjectOfType<TestLobby>().Disconnect();
    }

    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}
