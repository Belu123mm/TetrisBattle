using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TetraFabric fabric;
    public Tetramino currentTetra;
    public float previousTime;     //esto es local no?
    public float falltime = 0.8f;   //Esto se va cambiando con elnivel
    public float aumFall = 0.1f;
    public int height;      //de la grid
    public int width;
    Vector3 zero { get { return transform.position; } }
    public int newTetra;
    public Transform [,] grid;
    public int lines;
    public int level= 1;
    //Hay una forma de calcular el puntaje, pero depende de la altura a la que colocas la pieza y el falltimey el valor de la pieza, a menos que todas valgan lo mismo
    

    /*Notas:
     * Instantiate (Resources.Load ("nombredelapieza") as GameObject); //Esto es el photonnetwork.instantiate :3
     * 
     * 
     */


    // Start is called before the first frame update
    void Start()
    {
        grid = new Transform [ width, height ];
        fabric.startPosition = new Vector3(width / 2, height - 2, 0);
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {//Estem update en teoria va a redes/controles
        if ( Input.GetButtonDown("Horizontal") ) {
            MoveTetraHorizontal(new Vector3(Input.GetAxisRaw("Horizontal"),0,0));
        }
        //El calculo del tiempo es en el server?, este lo hace caer constantemente
        //Los controles son tipo flechitas y z :3
        if (Input.GetKey(KeyCode.DownArrow)) {
            MoveTetraKeyDown();
        } else {
            MoveTetraIdleDown();
        }

        if ( Input.GetKeyDown(KeyCode.Z) ) {
            RotateTetra();
        }

    }

        //Esto se le pide al server, y inicia a la vez con todos
    void StartGame() {
        newTetra = NewTetraIndex();
        currentTetra = fabric.SpawnTetra(newTetra);

    }

    void MoveTetraHorizontal(Vector3 dir) {
        currentTetra.transform.position += dir;
        if ( !isValidMove() ) {
            currentTetra.transform.position += -dir;

        }
    }
    void MoveTetraIdleDown() {
        if ( Time.time - previousTime > falltime ) {
            currentTetra.transform.position += Vector3.down;
            previousTime = Time.time;
            if ( !isValidMove() ) {
                currentTetra.transform.position -= Vector3.down;
                AddToGrid(currentTetra);
                CheckForLines();
                newTetra = NewTetraIndex();
                currentTetra = fabric.SpawnTetra(newTetra);
            }
        }

    }

    void MoveTetraKeyDown() {
        if ( Time.time - previousTime > falltime/10 ) {
            currentTetra.transform.position += Vector3.down;
            previousTime = Time.time;
            if ( !isValidMove() ) {
                currentTetra.transform.position -= Vector3.down;
                AddToGrid(currentTetra);
                CheckForLines();
                newTetra = NewTetraIndex();
                currentTetra = fabric.SpawnTetra(newTetra);
            }
        }



    }
    //ESTE SE LO PIDO AL SERVER, ASI TODOS TIENEN EL MISMO NUMERO YOU KNOW, en teoria, esto de pedirle a la fabrica seugn el numero y esas cosas me tiene confundida, pero se va a aclarar cuando meta esto a redes
    int NewTetraIndex() {
        return Random.Range(0, 7);
    }
    //Es todo muy confuso
    void AddToGrid( Tetramino tetra) {
        foreach (Transform block in tetra.transform ) {
            int roundX = Mathf.RoundToInt(block.transform.position.x - zero.x) ;
            int roundY = Mathf.RoundToInt(block.transform.position.y - zero.y);
            //Debug.Log(roundX + " add to grid " + roundY);
            grid [ roundX, roundY ] = block;


        }
    }


    void RotateTetra() {
        currentTetra.transform.RotateAround(currentTetra.transform.TransformPoint(currentTetra.pivot), Vector3.forward, 90);
        if (!isValidMove())
            currentTetra.transform.RotateAround(currentTetra.transform.TransformPoint(currentTetra.pivot), Vector3.forward, -90);
    }

    bool isValidMove() {
        foreach(Transform block in currentTetra.transform ) {
            int roundX = Mathf.RoundToInt(block.transform.position.x - zero.x);
            int roundY = Mathf.RoundToInt(block.transform.position.y - zero.y);
            //Debug.Log(roundX + " isvalid " + roundX);

            if ( roundX < 0||roundX >= width || roundY < 0|| roundY>= height ) {
                return false;
            }
            if (grid[ Mathf.RoundToInt(roundX - zero.x), Mathf.RoundToInt(roundY - zero.y)] != null ) {
                return false;
            }

        }
        return true;
    }

    void CheckForLines() {
        for ( int i = height - 1; i >= 0; i-- ) {
            if ( HasLine(i) ) {
                DeleteLine(i);
                CheckLevel();
                RowDown(i);
            }
        }
    }

    bool HasLine( int i ) {
        for ( int j = 0; j < width; j++ ) {
            if ( grid [ j, i ] == null )
                return false;
        }

        return true;
    }

    void DeleteLine( int i ) {
        for ( int j = 0; j < width; j++ ) {
            grid [ j, i ].parent.gameObject.GetComponent<Tetramino>().DestroyBlock(grid [ j, i ]); ;
            //Destroy(grid [ j, i ].gameObject);//Aca llamo al bloque/parent
            grid [ j, i ] = null;
        }
    }

    void RowDown( int i ) {
        for ( int y = i; y < height; y++ ) {
            for ( int j = 0; j < width; j++ ) {
                if ( grid [ j, y ] != null ) {
                    grid [ j, y - 1 ] = grid [ j, y ];
                    grid [ j, y ] = null;
                    grid [ j, y - 1 ].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    void CheckLevel() {
        lines++;
        float resto = lines % 30;
        if (resto == 0 ) {
            LevelUp();
        }
    }
    void LevelUp() {
        level++;
        falltime -= aumFall;
    }

    //Esto es de debug noms
    //Altos gizmos papu
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.up * height + Vector3.left * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.right * width + Vector3.left * 0.5f, transform.position + Vector3.up * height+ Vector3.right * width + Vector3.left * 0.5f);

        Gizmos.DrawLine(transform.position + Vector3.left * 0.5f, transform.position + Vector3.right * width + Vector3.left * 0.5f);
        Gizmos.DrawLine(transform.position + Vector3.up * height + Vector3.left * 0.5f, transform.position + Vector3.right * width + Vector3.up * height + Vector3.left * 0.5f);
        
    }
}
