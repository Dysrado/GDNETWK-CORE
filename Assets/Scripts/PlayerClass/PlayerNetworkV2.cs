using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Netcode;

using System.Globalization;
using UnityEngine.UIElements;

public class PlayerNetworkV2 : NetworkBehaviour
{
    [SerializeField] private bool _serverAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;
    [SerializeField] private SkinnedMeshRenderer[] meshes;
    [SerializeField] private GameObject _nameTag;
    

    private readonly NetworkVariable<Color> NetColor = new();
    private readonly Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.cyan, Color.gray };
    int index;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private Rigidbody _rb;


    [SerializeField] NetworkVariable<NetworkString> username = new NetworkVariable<NetworkString>();

    // Start is called before the first frame update
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        var permission = _serverAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: NetworkVariableWritePermission.Owner);

        
    }

    private void Start()
    {
        if (IsOwner)
            SetUsernameServerRpc(GameObject.FindGameObjectWithTag("Username").GetComponent<TMPro.TMP_Text>().text);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) 
            this.enabled = false;

        
        GameObject nameTag = Instantiate(_nameTag, GameObject.FindGameObjectWithTag("WorldSpace").transform);
        nameTag.GetComponent<NameTagBehaviour>().SetReference(this.gameObject);
        

        index = (int)OwnerClientId;

        //Added Change
        this.gameObject.GetComponent<PlayerManager>().SetInfo(index, nameTag.GetComponent<NameTagBehaviour>());


        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material.color = colors[index % colors.Length];
        }
    }

    public int GetNetworkID()
    {
        return (int)OwnerClientId;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            TransmitState();

        }
        
        else { 
            ConsumeState();
        }


    }

    #region Transmit State
    private void TransmitState()
    {
        var state = new PlayerNetworkState
        {
            Position = _rb.position,
            Rotation = transform.rotation.eulerAngles
        };



        if (IsServer || !_serverAuth)
            _playerState.Value = state;
        else
            TransmitStateServerRpc(state);
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state)
    {
        _playerState.Value = state;
    }

    [ServerRpc]
    private void SetUsernameServerRpc(NetworkString pname)
    {
        username.Value = pname;

        //Added changes = only works at the HOST not on the client
        //this.gameObject.GetComponent<PlayerManager>().SetUserName(pname);
    }


    public string GetUsername() { return username.Value.ToString(); }

    #endregion

    #region Interpolate State

    private Vector3 _posVel;
    private float _rotVelY;

    private void ConsumeState()
    {
        // Here you'll find the cheapest, dirtiest interpolation you'll ever come across. Please do better in your game
        _rb.MovePosition(Vector3.SmoothDamp(_rb.position, _playerState.Value.Position, ref _posVel, _cheapInterpolationTime));

        transform.rotation = Quaternion.Euler(
            0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _playerState.Value.Rotation.y, ref _rotVelY, _cheapInterpolationTime), 0);
    }
    #endregion

    private struct PlayerNetworkState : INetworkSerializable
    {
        private float _posX, _posZ;
        private short _rotY;

        internal Vector3 Position
        {
            get => new(_posX, 0, _posZ);
            set
            {
                _posX = value.x;
                _posZ = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new(0, _rotY, 0);
            set => _rotY = (short)value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _posX);
            serializer.SerializeValue(ref _posZ);

            serializer.SerializeValue(ref _rotY);
        }
    }

    public struct NetworkString : INetworkSerializable
    {
        private FixedString32Bytes info;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref info); // SerializeValue(ref info) is giving the error
        }

        public override string ToString()
        {
            return info.ToString();
        }

        public static implicit operator string(NetworkString s) => s.ToString();
        public static implicit operator NetworkString(string s) => new NetworkString() { info = new FixedString32Bytes(s) };
    }

    
}
