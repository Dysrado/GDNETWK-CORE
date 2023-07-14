using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTagBehaviour : MonoBehaviour
{
    GameObject _reference;
    [SerializeField] private TMPro.TMP_Text username;
    [SerializeField] private Vector3 _offset;

    bool calledOnce = false;

    public void SetReference(GameObject pf) { _reference = pf; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_reference != null)
        {
            username.SetText(_reference.GetComponent<PlayerNetworkV2>().GetUsername()); // for some reason has to be called in the update function
            this.gameObject.transform.position = _reference.transform.position + _offset;
        }
        else { 
            Destroy(gameObject); 
        }
          
        

    }
}
