using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> NetPos = new (writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Quaternion> NetRot = new (writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Color> NetColor = new();
    private readonly Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white, Color.cyan, Color.gray};
    int index;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        index = (int)OwnerClientId;
        GetComponent<MeshRenderer>().material.color = colors[index % colors.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            NetPos.Value = transform.position;
            NetRot.Value = transform.rotation;
        }
        else
        {
            transform.position = NetPos.Value;
            transform.rotation = NetRot.Value;
        }
    }
}
