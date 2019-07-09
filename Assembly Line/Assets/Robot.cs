using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Robot : MonoBehaviourPun
{
    public int oilCant;
    public int lightsCant;
    public int gearCant;
    public int chipCant;

    public GameObject oil;
    public GameObject gears;
    public GameObject chips;
    public GameObject lights;

    public void Update() {
        if (oilCant == 0 ) {
            oil.SetActive(false);
        }
        if ( lightsCant == 0 ) {
            lights.SetActive(false);
        }
        if ( gearCant == 0 ) {
            gears.SetActive(false);
        }
        if ( chipCant == 0 ) {
            chips.SetActive(false);
        }

        if (oilCant == 0 && lightsCant == 0 && gearCant == 0 && chipCant == 0 ) {
            //nosvamos
        }

    }

}
