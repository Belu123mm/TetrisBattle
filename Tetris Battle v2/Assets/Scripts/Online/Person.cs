using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Person : MonoBehaviourPun {
    PhotonView _view;
    public bool started;

    void Start() {
        _view = GetComponent<PhotonView>();
        //Aca cuando creo el gamemanager y lo linkeo hago lo de las camaras 
        if ( !_view.IsMine )
            return;

        StartCoroutine(WaitToServer());


    }


    void Update() {/*
        if ( started == false ) {//??? esto va aca?

            if (! Server.Instance ) {
                Debug.Log("false");
            return;
            }
                Debug.Log(Server.Instance);
                Server.Instance.PlayerRequestToStart(PhotonNetwork.LocalPlayer);
                started = true;
        }
        */
        if ( !_view.IsMine )
            return;

        if ( Input.GetButtonDown("Horizontal") ) {
            Server.Instance.PlayerRequestMoveTetraSide(new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0), PhotonNetwork.LocalPlayer);
        }
    }

    IEnumerator WaitToServer() {
        yield return new WaitForSeconds(3);
        Server.Instance.ManagerRequestToStart(PhotonNetwork.LocalPlayer);
    }

}
//Correcto
