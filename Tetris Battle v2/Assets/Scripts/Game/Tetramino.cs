using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Tetramino : MonoBehaviourPun
{
    public Vector3 pivot;
    public int blockCount = 4;

    public void DestroyBlock( Transform block ) {
        blockCount--;
        PhotonNetwork.Destroy(block.gameObject);
        if ( blockCount == 0 ) {
            Debug.Log("tetra destroyed uwu");
            PhotonNetwork.Destroy(this.gameObject);
        }

    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position + pivot + Vector3.back, 0.25f);
    }
}
