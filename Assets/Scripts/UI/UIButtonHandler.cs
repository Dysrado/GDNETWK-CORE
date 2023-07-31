using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonHandler : MonoBehaviour
{
    [SerializeField] GameObject Map;
    [SerializeField] GameObject TitleUI;
    [SerializeField] GameObject LobbyUI;
    [SerializeField] GameObject EndScreenUI;
    
    public void LeaveGame()
    {
        Map.SetActive(false);
        EndScreenUI.SetActive(false);
        TitleUI.SetActive(true);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
