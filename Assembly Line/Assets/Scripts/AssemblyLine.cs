using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AssemblyLine : MonoBehaviourPun {
    public Collider cinta;
    PhotonView _view;
    public void Awake() {
        _view = GetComponent<PhotonView>();
    }
    public void OnTriggerEnter( Collider other ) {
        if ( !_view.IsMine ) return;
    }
}

