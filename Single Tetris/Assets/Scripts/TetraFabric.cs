using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraFabric : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 startPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Tetramino SpawnTetra(int n) {
        var newtetra = Resources.Load("Tetra" + n) as GameObject;
        var t = Instantiate( newtetra,startPosition,Quaternion.identity);
        return t.GetComponent<Tetramino>();
    }
}
