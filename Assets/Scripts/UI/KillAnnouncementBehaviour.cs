using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KillAnnouncementBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameTxt;
    private float decay = 7.0f;
    private float time = 0.0f;
    


    // Update is called once per frame
    void Update()
    {
        if (time > decay)
        {
            Destroy(this.gameObject);
        }
        time += Time.deltaTime;
    }

    public void InsertText(string memssage)
    {
        gameTxt.text = memssage;
    }
}
