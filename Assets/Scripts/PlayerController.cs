using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 250.0f;
    private Vector3 moveInput;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner) Destroy(this);
    //}
    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        rb.velocity = moveInput.normalized * speed * Time.deltaTime;
    }
}
