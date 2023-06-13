using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rb;
    private Camera mainCamera;

    [SerializeField] private float speed = 250.0f;

    private Vector3 moveInput;
    private Vector3 mousePos;

    private string playerName;
    Vector3 lookDirection = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
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

        mousePos = Input.mousePosition;
        Ray r = mainCamera.ScreenPointToRay(mousePos);
        

        if (Physics.Raycast(r, out RaycastHit hit))
        {
            lookDirection = hit.point;

        }
        transform.LookAt(new Vector3(lookDirection.x, transform.position.y, lookDirection.z));
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveInput.x, 0, moveInput.z).normalized * speed * Time.deltaTime;
    }

    public string GetPlayerName() { return playerName; }

    public void SetPlayerName(string name) { playerName = name; }
}
