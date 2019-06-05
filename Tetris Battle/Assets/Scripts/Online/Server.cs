using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class Server : MonoBehaviourPunCallbacks
{
    int maxPersons;
    public Server (int persons ) {
        maxPersons = persons;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
