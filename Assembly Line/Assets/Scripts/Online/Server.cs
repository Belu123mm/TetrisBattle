using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class Server : MonoBehaviourPun {
    public static Server Instance { get; private set; } //El private es para que no te hackeen y que lo seteen(xd) de otro lado 
    public Dictionary<Player, Assembler> assemblers = new Dictionary<Player, Assembler>();
    PhotonView _view;
    public Player server;
    public bool inGame = false;
    public Vector3 robotsPosition;
    // Start is called before the first frame update
    public Robot client;
    public int level = 1;
    public TMP_Text lifeText;
    public int life;
    public int maxLevel;

    void Awake() {
        _view = GetComponent<PhotonView>();


        if ( !Instance ) {
            if ( _view.IsMine ) {
                _view.RPC("SetReferenceToSelf", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);  //Buffered: todos los que se conecten tarde reciben este mensaje
            }

        } else {
            PhotonNetwork.Destroy(gameObject);  //Me destrushe
        }

    }
    private void Update() {
        lifeText.text = life.ToString();

        if ( !_view.IsMine ) return;
        if ( inGame == false ) {
            int max = PhotonNetwork.CurrentRoom.MaxPlayers;
            int curr = PhotonNetwork.CurrentRoom.PlayerCount;
            if ( max == curr ) {
                inGame = true;
            }
            return;
        }
        if ( client == null ) {
            int r = Random.Range(0, 5);
            if ( r == 4 ) {
                int robotid = PhotonNetwork.Instantiate("RoboSphere", robotsPosition, Quaternion.identity).GetComponent<PhotonView>().ViewID;
                int o = Mathf.RoundToInt(level * 0.25f);
                int c = Mathf.RoundToInt(level * 0.80f);
                int g = Mathf.RoundToInt(level * 0.60f);
                int l = Mathf.RoundToInt(level * 1.25f);
                _view.RPC("AskToFindRoboSphere", RpcTarget.All, robotid, o, c, g, l);

            }
        }

    }

    [PunRPC]    //Funcion que se llama enel servidory se replica en todos los clientes 
    public void SetReferenceToSelf( Player p ) {    //Esto es como un singleton pero para redes
        Instance = this;
        server = p;

        DontDestroyOnLoad(this);
        if ( !PhotonNetwork.IsMasterClient )    //Wachin, sos el master? (osea quien hostea la partida)
        {
            _view.RPC("AddPlayer", server, PhotonNetwork.LocalPlayer);//Flasheaste, te agrego un player
        }

    }

    [PunRPC]
    public void AddPlayer( Player p ) { //Esto agrega los de otros lados?

        if ( !_view.IsMine )
            return;     //Wat is dis?
        var newPlayer = PhotonNetwork.Instantiate("Assembler", Vector3.left * p.ActorNumber * 2 ,
                        Quaternion.identity).GetComponent<Assembler>();
        assemblers.Add(p, newPlayer);
        //Aca inicias el tiempo
    }

    //Los Ask son rpc

    [PunRPC]
    public void AskAssemblerToMoveHorizontal( Vector3 dir, Player p ) {
        if ( !_view.IsMine ) return;
        if ( assemblers.ContainsKey(p) ) {
            assemblers [ p ].MoveHorizontal(dir);
        }
    }
    [PunRPC]
    public void AskAssemblerToMoveVertical( Vector3 dir, Player p ) {

        if ( !_view.IsMine ) return;
        if ( assemblers.ContainsKey(p) ) {
            assemblers [ p ].MoveVertical(dir);
        }
    }
    [PunRPC]
    public void AskAssemblerToGrab( Player p ) {
        if ( !_view.IsMine ) return;

        if ( assemblers.ContainsKey(p) ) {
            int assembler = assemblers [ p ].photonView.ViewID;
            if ( assemblers [ p ].isGrabbin == false ) {

                RaycastHit hit;
                if ( Physics.Raycast(assemblers [ p ].transform.position + Vector3.up, assemblers [ p ].transform.forward, out hit, 10, assemblers [ p ].gearsVM) ) {

                    int grabbb = PhotonNetwork.Instantiate("Gear", assemblers [ p ].transform.position + assemblers [ p ].transform.forward, Quaternion.identity).GetComponent<PhotonView>().ViewID;
                    //aca tengo que darselo a todos los chikes para que lo encuentren y lo agarren
                    _view.RPC("AskToGrabThis", RpcTarget.All, grabbb, assembler);
                }
                if ( Physics.Raycast(assemblers [ p ].transform.position + Vector3.up, assemblers [ p ].transform.forward, out hit, 10, assemblers [ p ].oilVM) ) {

                    int grabbb = PhotonNetwork.Instantiate("Oil", assemblers [ p ].transform.position + assemblers [ p ].transform.forward, Quaternion.identity).GetComponent<PhotonView>().ViewID;
                    //aca tengo que darselo a todos los chikes para que lo encuentren y lo agarren
                    _view.RPC("AskToGrabThis", RpcTarget.All, grabbb, assembler);
                }
                if ( Physics.Raycast(assemblers [ p ].transform.position + Vector3.up, assemblers [ p ].transform.forward, out hit, 10, assemblers [ p ].chipVM) ) {

                    int grabbb = PhotonNetwork.Instantiate("Chip", assemblers [ p ].transform.position + assemblers [ p ].transform.forward, Quaternion.identity).GetComponent<PhotonView>().ViewID;
                    //aca tengo que darselo a todos los chikes para que lo encuentren y lo agarren
                    _view.RPC("AskToGrabThis", RpcTarget.All, grabbb, assembler);
                }
                if ( Physics.Raycast(assemblers [ p ].transform.position + Vector3.up, assemblers [ p ].transform.forward, out hit, 10, assemblers [ p ].lightsVM) ) {

                    int grabbb = PhotonNetwork.Instantiate("Lights", assemblers [ p ].transform.position + assemblers [ p ].transform.forward, Quaternion.identity).GetComponent<PhotonView>().ViewID;
                    //aca tengo que darselo a todos los chikes para que lo encuentren y lo agarren
                    _view.RPC("AskToGrabThis", RpcTarget.All, grabbb, assembler);
                }


            } else {
                _view.RPC("AskToGrabThis", RpcTarget.All, 0, assembler);
            }

        }
    }
    [PunRPC]
    void AskToGrabThis( int itemid, int assemblerid) {
        Assembler a = PhotonView.Find(assemblerid).GetComponent<Assembler>();
        if ( itemid != 0 ) {

            a.grabbed = PhotonView.Find(itemid).GetComponent<Transform>();
            a.isGrabbin = true;

        } else {
            a.isGrabbin = false;
        }
    }
    [PunRPC]
    void AskToAnimAssembler(Player p) {
        if ( !_view.IsMine ) return;

        if ( assemblers.ContainsKey(p) )
            assemblers [ p ].SetMoving(true);
    }
    [PunRPC]
    void AskToStopAnim( Player p ) {
        if ( !_view.IsMine ) return;

        if ( assemblers.ContainsKey(p) )
            assemblers [ p ].SetMoving(false);
    }
    [PunRPC]
    void AskToFindRoboSphere(int roboid ,int oil, int chip,int gear, int lights) {
        client = PhotonView.Find(roboid).GetComponent<Robot>();
        client.oilCant = oil;
        client.chipCant = chip;
        client.gearCant = gear;
        client.lightsCant = lights;
    }
    [PunRPC]
    void AskRobotToDeploy() {
        client._anim.SetBool("isLeaving", true);
        if ( !_view.IsMine ) return;
        client.Deploy();
    }
    [PunRPC]
    void LessLife() {
        life--;
        if ( !_view.IsMine ) return;
        if ( life == 0 ) {
            PhotonNetwork.LoadLevel("Lose");
            PhotonNetwork.Disconnect();
        }
    }
    [PunRPC]
    void LessOil() {
        client.oilCant--;
    }
    [PunRPC]
    void LessGears() {
        client.gearCant--;
    }
    [PunRPC]
    void LessLights() {
        client.lightsCant--;
    }
    [PunRPC]
    void LessChips() {
        client.chipCant--;
    }
    [PunRPC]
    void LevelUp() {
        level++;
        if ( !_view.IsMine ) return;
        if ( level == maxLevel ) {
            PhotonNetwork.LoadLevel("Win");
            PhotonNetwork.Disconnect();
        }
    }

    //Los request no son rpc
    public void PlayerRequestToMoveHorizontal(Vector3 dir,Player p ) {
        _view.RPC("AskAssemblerToMoveHorizontal", server, dir, p);
        _view.RPC("AskToAnimAssembler", RpcTarget.All, p);
    }
    public void PlayerRequestToMoveVertical( Vector3 dir, Player p ) {
        _view.RPC("AskAssemblerToMoveVertical", server, dir, p);
        _view.RPC("AskToAnimAssembler", RpcTarget.All, p);
    }
    public void PlayerRequestToGrab(Player p ) {
        _view.RPC("AskAssemblerToGrab", server, p);
    }
    public void PlayerRequestToStopAnim( Player p ) {
        _view.RPC("AskToStopAnim", RpcTarget.All, p);
    }
    public void RobotRequestToChangeRobot() {
        _view.RPC("LevelUp", RpcTarget.All);
        _view.RPC("AskRobotToDeploy", RpcTarget.All);
    }



    public void OnTriggerEnter( Collider other ) {
        if ( !_view.IsMine ) return;

        if ( other.CompareTag("oil")){
            PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
            if ( client.oilCant == 0 ) {
                _view.RPC("AskRobotToDeploy", RpcTarget.All);
                _view.RPC("LessLife", RpcTarget.All);
                return;
            }
            _view.RPC("LessOil", RpcTarget.All);
        }

        if ( other.CompareTag("gear") ) {
            PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
            if ( client.gearCant == 0 ) {
                _view.RPC("AskRobotToDeploy", RpcTarget.All);
                _view.RPC("LessLife", RpcTarget.All);
                return;
            }
            _view.RPC("LessGears", RpcTarget.All);
        }

        if ( other.CompareTag("chip") ) {
            PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
            if ( client.chipCant == 0 ) {
                _view.RPC("AskRobotToDeploy", RpcTarget.All);
                _view.RPC("LessLife", RpcTarget.All);
                return;
            }
            _view.RPC("LessChips", RpcTarget.All);
        }

        if ( other.CompareTag("lights") ) {
            PhotonNetwork.Destroy(other.GetComponent<PhotonView>());
            if ( client.lightsCant == 0 ) {
                _view.RPC("AskRobotToDeploy", RpcTarget.All);
                _view.RPC("LessLife", RpcTarget.All);
                return;
            }
            _view.RPC("LessLights", RpcTarget.All);
        }

    }
}
