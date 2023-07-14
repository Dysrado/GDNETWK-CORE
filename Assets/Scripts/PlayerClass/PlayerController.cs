using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rb;
    private Camera mainCamera;

    [SerializeField] private float speed = 250.0f;
    private float rotationSpeed = 1000.0f;

    private Vector3 moveInput;
    private Vector3 mousePos;

    [SerializeField] Animator _anim;

    [SerializeField] KeyCode DASH_BUTTON = KeyCode.Space;
    [SerializeField] KeyCode ALT_DASH_BUTTON = KeyCode.LeftShift;

    [SerializeField] float _dashDuration = 0.2f; // Duration of the dash
    [SerializeField] float _dashCooldown = 0.5f; // Time you can do the next dash
    [SerializeField] float DASH_MULTIPLIER = 2.0f; // For how many seconds is the dash

    Vector3 _dashDirection = Vector3.zero; // Direction of the dash
    float _dashSpeed;

    bool _canDash = true; // Can the player dash
    bool _isDashing = false; // Is currently dashing

    [SerializeField] NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        _dashSpeed = speed * DASH_MULTIPLIER;
        
    }

    private void Start()
    {
        
    }

    //public override void OnNetworkSpawn()
    //{
    //    if (!IsOwner) Destroy(this); 
    //}
    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        Ray r = mainCamera.ScreenPointToRay(mousePos);

        if (Input.GetKeyDown(DASH_BUTTON) || Input.GetKeyDown(ALT_DASH_BUTTON))
        {
            if (!_isDashing && _canDash)
            {
                Dash(_dashDuration); // The actual dash
                StartDashCD(_dashCooldown); // When Dash() finishes do StartDashCD()
            }
        }
        else
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.z = Input.GetAxisRaw("Vertical");

        }


        if (IsOwner)
        {
            if (_isDashing) 
            {
                UpdatePlayerStateServerRpc(PlayerState.DASHING);
            }
            else if (moveInput != Vector3.zero)
            {
                UpdatePlayerStateServerRpc(PlayerState.RUNNING);
            }
            else if(moveInput == Vector3.zero)
            {
                UpdatePlayerStateServerRpc(PlayerState.IDLE);
            }
        }
        

        // Handles Animations
        if (networkPlayerState.Value == PlayerState.IDLE)
        {
            _anim.SetBool("IsRunning", false);
            _anim.SetBool("IsDashing", false);

        }
        else if (networkPlayerState.Value == PlayerState.RUNNING)
        {
            _anim.SetBool("IsRunning", true);
            _anim.SetBool("IsDashing", false);

        }
        if (networkPlayerState.Value == PlayerState.DASHING)
        {
            _anim.SetBool("IsDashing", true);

        }

        // Look at
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            if (hit.collider.gameObject != this.gameObject)
            {
                var lookDirection = hit.point - transform.position;
                var rotation = Quaternion.LookRotation(new Vector3(lookDirection.x, transform.position.y, lookDirection.z));

                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
            

        }
    }

    private void FixedUpdate()
    {
        if (!_isDashing)
        {
            rb.velocity = new Vector3(moveInput.x, 0, moveInput.z).normalized * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector3(_dashDirection.x, 0, _dashDirection.z).normalized * _dashSpeed * Time.deltaTime;

        }
    }


    // async = coroutine
    // Gets the moveInput then dashes to the direction for "duration" amount of seconds
    public async void Dash(float duration) 
    {
        var endtime = Time.time + duration;
        _dashDirection = moveInput;
        while (Time.time < endtime)
        {
            _isDashing = true;
            await Task.Yield();
            _isDashing = false;
            _canDash = false;
            Debug.Log($"Dashed - candash: {_canDash}");
        }
    }

    // Starts the timer for the cooldown
    public async void StartDashCD(float duration)
    {
        var endtime = Time.time + duration;
        while(Time.time < endtime)
        {
            _canDash = false;
            await Task.Yield();
            _canDash = true;
        }
    }

    [ServerRpc]
    public void UpdatePlayerStateServerRpc(PlayerState state)
    {
        networkPlayerState.Value = state;
    }



    public enum PlayerState
    {
        IDLE = 0,
        RUNNING,
        DASHING,
        DEAD
    }



 

}
