using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Person : MonoBehaviourPun {
    PhotonView _view;

    void Start() {
        _view = GetComponent<PhotonView>();
        //Aca cuando creo el gamemanager y lo linkeo hago lo de las camaras 
    }


    void Update() {
        if ( !_view.IsMine )
            return;
    }
}
//Correcto
