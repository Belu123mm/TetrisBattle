using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Controller : MonoBehaviourPun
{
    PhotonView _view;

    private void Start() {
        _view = GetComponent<PhotonView>();
    }
    private void Update() {
        if ( !_view.IsMine ) return;

        if ( Input.GetButton("Horizontal")){
            Debug.Log("mandando para mover");
            Server.Instance.PlayerRequestToMoveHorizontal(Vector3.right * Input.GetAxis("Horizontal"), PhotonNetwork.LocalPlayer);
        }
        if ( Input.GetButton("Vertical") ) {
            Debug.Log("mandando para mover");
            Server.Instance.PlayerRequestToMoveVertical(Vector3.forward * Input.GetAxis("Vertical"), PhotonNetwork.LocalPlayer);
        }
        if ( Input.GetKeyDown(KeyCode.E) ) {
            Server.Instance.PlayerRequestToGrab(PhotonNetwork.LocalPlayer);
        }

    }
}
