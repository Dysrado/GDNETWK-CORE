using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;


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

    public void SetName(string name)
    {
        playerName.text = name;
    }

    public void Leave()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
