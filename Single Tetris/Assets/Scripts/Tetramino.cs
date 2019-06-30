using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetramino : MonoBehaviour
{
    public Vector3 pivot;
    public int blockCount = 4;

    public void DestroyBlock(Transform block) {
        blockCount--;
        Destroy(block.gameObject);
        if ( blockCount == 0 ) {
            Debug.Log("tetra destroyed uwu");
            Destroy(this.gameObject);
        }

    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position + pivot + Vector3.back, 0.25f);
    }
}
