using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TetraFactory : MonoBehaviourPun
{
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Tetramino SpawnTetra( int n ) {
        var t = PhotonNetwork.Instantiate("Tetra" + n, startPosition, Quaternion.identity);
        return t.GetComponent<Tetramino>();
    }

}
