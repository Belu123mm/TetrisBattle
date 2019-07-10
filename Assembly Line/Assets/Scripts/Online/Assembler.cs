using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Assembler : MonoBehaviourPun {
    public float movementSpeed;
    public float rotateSpeed;
    public Vector3 direction;
    public bool isGrabbin;
    public Transform grabbed;
    PhotonView _view;
    public LayerMask gearsVM;
    public LayerMask oilVM;
    public LayerMask chipVM;
    public LayerMask lightsVM;
    bool _isMovingHor;
    bool _isMovingVer;
    Animator _anim;

    public void Awake() {
        _view = GetComponent<PhotonView>();
        _anim = GetComponent<Animator>();

    }
    public void MoveHorizontal(Vector3 dir ) {
        if ( !_isMovingHor ) {
            _isMovingHor = true;

            transform.position += dir * movementSpeed * Time.deltaTime;
            Vector3 targetDir = (transform.position + dir) - transform.position;

            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            transform.rotation = Quaternion.LookRotation(newDir);
            StartCoroutine(WaitToMoveHor());

        }
    }
    public void MoveVertical( Vector3 dir ) {
        if ( !_isMovingVer ) {
            _isMovingVer = true;

            transform.position += dir * movementSpeed * Time.deltaTime;
            Vector3 targetDir = (transform.position + dir) - transform.position;

            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            transform.rotation = Quaternion.LookRotation(newDir);
            StartCoroutine(WaitToMoveVer());

        }
    }


    IEnumerator WaitToMoveHor() {
        yield return new WaitForFixedUpdate();
        _isMovingHor = false;
    }
    IEnumerator WaitToMoveVer() {
        yield return new WaitForFixedUpdate();
        _isMovingVer = false;
    }

    public void SetMoving( bool v ) {
        if ( _anim )
            _anim.SetBool("isMoving", v);
    }

    public void Update() {
        if ( isGrabbin ) {
            grabbed.position = transform.position + transform.forward + Vector3.up;
        }
    }

}
