using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
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

    public TMP_Text oilText;
    public TMP_Text gearsText;
    public TMP_Text chipsText;
    public TMP_Text lightsText;

    public Animator _anim;

    bool deployed = false;
    PhotonView _view;

    public void Awake() {
        _view = GetComponent<PhotonView>();
    }
    public void Update() {
        oilText.text = oilCant.ToString();
        gearsText.text = gearCant.ToString();
        chipsText.text = chipCant.ToString();
        lightsText.text = lightsCant.ToString();

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

        if ( !_view.IsMine ) return;
        if (oilCant == 0 && lightsCant == 0 && gearCant == 0 && chipCant == 0 && deployed == false) {
            deployed = true;
            Server.Instance.RobotRequestToChangeRobot();
        }

    }

    public void Deploy() {
        StartCoroutine(Deploying());
    }

    IEnumerator Deploying() {
        Debug.Log("BURRANDO");
        yield return new WaitForSeconds(2);
        PhotonNetwork.Destroy(this.gameObject);
    }


}
