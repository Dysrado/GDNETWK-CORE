using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;

public class BulletID : MonoBehaviour
{
    private readonly Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.cyan, Color.gray };

    protected int ownerId;
    // Start is called before the first frame update
    
    public void Initialize(int ID)
    {
        ownerId = ID;
        Debug.Log("client id:" + ownerId);
        GetComponent<MeshRenderer>().material.color = colors[ownerId % colors.Length];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
