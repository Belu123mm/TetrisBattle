using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviourPun {
    public static Server Instance { get; private set; } //El private es para que no te hackeen y que lo seteen(xd) de otro lado 
    public Dictionary<Player, Assembler> assemblers = new Dictionary<Player, Assembler>();
    PhotonView _view;
    public Player server;
    public bool inGame = false;
    // Start is called before the first frame update

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
        if ( !_view.IsMine ) return;
        if ( inGame == false ) {
            int max = PhotonNetwork.CurrentRoom.MaxPlayers;
            int curr = PhotonNetwork.CurrentRoom.PlayerCount;
            if ( max == curr ) {
                inGame = true;
                PhotonNetwork.Instantiate("TheLine", Vector3.zero + Vector3.up / 2, Quaternion.identity);
            }
            return;
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
        var newPlayer = PhotonNetwork.Instantiate("Assembler", Vector3.right * p.ActorNumber * 2 + Vector3.up,
                        Quaternion.identity).GetComponent<Assembler>();
        assemblers.Add(p, newPlayer);
        //Aca inicias el tiempo
        foreach ( var item in assemblers ) {
            Debug.Log(item);
        }
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
            Debug.Log("foundtheassembler");
            if ( assemblers [ p ].isGrabbin == false ) {
                Debug.Log("isfalse");

                RaycastHit hit;
                if ( Physics.Raycast(assemblers [ p ].transform.position, assemblers [ p ].transform.forward, out hit, 100, assemblers [ p ].layers) ) {

                    Debug.Log("found");
                    Transform grabbb = PhotonNetwork.Instantiate("Sphere", assemblers [ p ].transform.position + assemblers [ p ].transform.forward, Quaternion.identity).transform;
                    //aca tengo que darselo a todos los chikes para que lo encuentren y lo agarren
                    TellToGrab(grabbb,p);
                }
            } else {
                TellToGrab(null,p);
            }

        }
    }
    [PunRPC]
    void AskToGrabThis( Transform t, Player p ) {
        if ( assemblers.ContainsKey(p) ) {
            assemblers [ p ].grabbed = t;
            assemblers [ p ].isGrabbin = t;
        }
    }
        //Los request no son rpc
        public void PlayerRequestToMoveHorizontal(Vector3 dir,Player p ) {
        _view.RPC("AskAssemblerToMoveHorizontal", server, dir, p);
    }
    public void PlayerRequestToMoveVertical( Vector3 dir, Player p ) {
        _view.RPC("AskAssemblerToMoveVertical", server, dir, p);
    }
    public void PlayerRequestToGrab(Player p ) {
        _view.RPC("AskAssemblerToGrab", server, p);
    }
    public void TellToGrab(Transform t ,Player p) {
        _view.RPC("AskToGrabThis", server, t,p);
    }
}
