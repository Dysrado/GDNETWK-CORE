using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;

public class BulletInfo : MonoBehaviour
{
    private readonly Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.cyan, Color.gray };

    protected int ownerId;
    // Start is called before the first frame update
    private int damage = 0;
    
    public void Initialize(int ID)
    {
        ownerId = ID;
        //Debug.Log("client id:" + ownerId);
        GetComponent<MeshRenderer>().material.color = colors[ownerId % colors.Length];

    }

    public int GetOwnerId()
    {
        return ownerId;
    }

    public int GetBulletDamage()
    {
        return damage;
    }

    public void SetBulletDamage(int dmg)
    {
        damage = dmg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
