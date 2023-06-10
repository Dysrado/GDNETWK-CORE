using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<Vector3> NetPos = new (writePerm: NetworkVariableWritePermission.Owner);
    private readonly NetworkVariable<Quaternion> NetRot = new (writePerm: NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    void Start()
    {
        
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
