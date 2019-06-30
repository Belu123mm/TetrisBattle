using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{
    public int height;      //de la grid
    public int width;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.up * height + Vector3.left * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.right * width + Vector3.left * 0.5f, transform.position + Vector3.up * height + Vector3.right * width + Vector3.left * 0.5f);

        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.right * width + Vector3.left * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.up * height + Vector3.left * 0.5f, transform.position + Vector3.right * width + Vector3.up * height + Vector3.left * 0.5f);

    }
}
