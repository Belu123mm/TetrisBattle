using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Controller : MonoBehaviourPun {
    PhotonView _view;
    bool _horHasStopped;
    bool _verHasStopped;
    bool _animHastStopped;
    private void Start() {
        _view = GetComponent<PhotonView>();
    }
    private void Update() {
        if ( !_view.IsMine ) return;

        if ( Input.GetButton("Horizontal") ) {
            _horHasStopped = false;
            _animHastStopped = false;
            Server.Instance.PlayerRequestToMoveHorizontal(Vector3.right * Input.GetAxis("Horizontal"), PhotonNetwork.LocalPlayer);
        } else {
            _horHasStopped = true;
        }


        if ( Input.GetButton("Vertical") ) {
            _verHasStopped = false;
            _animHastStopped = false;
            Server.Instance.PlayerRequestToMoveVertical(Vector3.forward * Input.GetAxis("Vertical"), PhotonNetwork.LocalPlayer);
        } else {
            _verHasStopped = true;


        }
        if (_verHasStopped == true && _horHasStopped == true ) {
            if(_animHastStopped == false ) {
                Server.Instance.PlayerRequestToStopAnim(PhotonNetwork.LocalPlayer);
                _animHastStopped = true;
            }
        }

        if ( Input.GetKeyDown(KeyCode.E) ) {
            Server.Instance.PlayerRequestToGrab(PhotonNetwork.LocalPlayer);
        }

    }
}
