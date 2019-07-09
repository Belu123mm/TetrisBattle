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

            // The step size is equal to speed times frame time.
            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
            StartCoroutine(WaitToMoveHor());

        }
    }
    public void MoveVertical( Vector3 dir ) {
        if ( !_isMovingVer ) {
            _isMovingVer = true;

            transform.position += dir * movementSpeed * Time.deltaTime;
            Vector3 targetDir = (transform.position + dir) - transform.position;

            // The step size is equal to speed times frame time.
            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
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
            grabbed.position = transform.position + transform.forward;
        }
    }
    /*
    public void Grab() {    //hay 3 acciones, con z x c y son 3 raycast distintis y 3 funcones distintas //lies, vamos con los rotators y esas cosas 
        RaycastHit hit;
        if ( isGrabbin == false ) {

            if ( Physics.Raycast(transform.position, transform.forward, out hit, 100, layers) ) {

                Debug.Log("found");
                //aca tengo que darselo a todos los chikes para que lo encuentren y lo agarren
                Server.Instance.AssemblerRequestAThing("Sphere", transform, PhotonNetwork.LocalPlayer);                //hit.transform.gameObject.GetComponent<>
            }
        } else {
            grabbed = null;
            isGrabbin = false;
        }
    }
    */
}
