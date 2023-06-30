using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private float bulletLifetime = 0.6f;
    private float bulletTicks = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bulletTicks += Time.deltaTime;
        if(bulletTicks > bulletLifetime)
        {
            this.gameObject.SetActive(false);
            bulletTicks = 0.0f;
        }
    }
}
