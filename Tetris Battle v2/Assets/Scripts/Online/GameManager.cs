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
    public bool camerasinplace;
    public List<Camera> cameras = new List<Camera>();
    public TetraFactory factory;
    public Transform [,] grid;
    public Tetramino currentTetra;
    public int newTetra;
    public Queue<int> tetraQueue = new Queue<int>();

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        /*
        if ( camerasinplace == false ) {
            int players = PhotonNetwork.CurrentRoom.MaxPlayers - 1;
            var c = FindObjectsOfType<Camera>();
            foreach ( var camera in c ) {
                if ( camera != cam ) {
                    cameras.Add(camera);

                }
            }
            Debug.Log("Cameras: " + cameras.Count + " - Players: " + players);
            if ( cameras.Count == (players - 1 )) {
                for ( int i = 0; i < cameras.Count; i++ ) {
                    //2,3,4
                    float a = (cameras.Count );
                    float h = 1 / a;
                    float y = (a ) * h;

                    Rect camerarect = new Rect(0.5f, y, 0.5f, h);
                    cameras [ i ].rect = camerarect;
                    camerasinplace = true;
                }
            }
        }
        */
        
    }
    public void Start() {
    }

    //esto se lo pedis al server, pero de ahi veo bien
    public void NewTetraIndex(int r) {
        tetraQueue.Enqueue(r);
        Debug.Log("newindex");
    }

    public void NewTetra() {
        newTetra = tetraQueue.Dequeue();
        currentTetra = factory.SpawnTetra(newTetra);
        Debug.Log("newtetra");
    }
    public void MoveTetraHorizontal( Vector3 dir ) {
        currentTetra.transform.position += dir;
        Debug.Log("moving the tetra");
        /*
        if ( !isValidMove() ) {
            currentTetra.transform.position += -dir;

        }
        */
    }
    public void SetStartPosition() {
        grid = new Transform [ width, height ];
        factory.startPosition = new Vector3(width / 2, height - 2, 0);
        Debug.Log("factoryPosition");
    }



    public void ReshapeCamera(int actornumber) {
        Debug.Log("Numero de actor: " + actornumber);
        float a = (actornumber - 1);
        float h = 1 / a;
        float y = (a - 1) * h;
        
        Rect camerarect = new Rect(0.5f,y, 0.5f,h);
        cam.rect = camerarect;
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.up * height + Vector3.left * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.right * width + Vector3.left * 0.5f, transform.position + Vector3.up * height + Vector3.right * width + Vector3.left * 0.5f);

        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.right * width + Vector3.left * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.up * height + Vector3.left * 0.5f, transform.position + Vector3.right * width + Vector3.up * height + Vector3.left * 0.5f);

    }
}
