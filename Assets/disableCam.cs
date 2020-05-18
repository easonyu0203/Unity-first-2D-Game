using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class disableCam : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] compoentsDisable;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < compoentsDisable.Length; i++)
            {
                compoentsDisable[i].enabled = false;
            }
        }
    }

}
