using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class BulletID : MonoBehaviour
{
    protected int ownerId;
    // Start is called before the first frame update
    public void Initialize(int ID)
    {
        ownerId = ID;
        Debug.Log("client id:" + ownerId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
