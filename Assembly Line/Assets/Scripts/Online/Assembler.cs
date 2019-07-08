using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Assembler : MonoBehaviourPun
{
    public float speed;

    public void MoveHorizontal(Vector3 dir ) {
        transform.position += dir * speed * Time.deltaTime;
    }
    public void MoveVertical( Vector3 dir ) {
        transform.position += dir * speed * Time.deltaTime;
    }

    public void Grab() {
        Debug.Log("grabbin");
    }
}
