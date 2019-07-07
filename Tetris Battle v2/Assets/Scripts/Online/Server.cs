using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviourPun {
    public static Server Instance { get; private set; } //El private es para que no te hackeen y que lo seteen(xd) de otro lado 
    public Dictionary<Player, GameManager> managers = new Dictionary<Player, GameManager>();
    PhotonView _view;
    public Player server;
    public int players;
    public bool startedGame;
    
    // Start is called before the first frame update
    
    void Awake()
    {
        _view = GetComponent<PhotonView>();
0

        if ( !Instance ) {
            if ( _view.IsMine ) {
                _view.RPC("SetReferenceToSelf", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);  //Buffered: todos los que se conecten tarde reciben este mensaje
            }

        }
        else {
            PhotonNetwork.Destroy(gameObject);  //Me destrushe

        }

    }

    [PunRPC]    //Funcion que se llama enel servidory se replica en todos los clientes 
    public void SetReferenceToSelf( Player p ) {    //Esto es como un singleton pero para redes
        Instance = this;
        server = p;
        players = PhotonNetwork.CurrentRoom.MaxPlayers-1;

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
        //Aca tengo que poner el tema ese de visited o no
        var newPlayer = PhotonNetwork.Instantiate("GameManager", Vector3.right * p.ActorNumber * 25,
                        Quaternion.identity).GetComponent<GameManager>();
        //Te menti, aca hago lo de la camara
        players--;
        managers.Add(p, newPlayer);

    }

    //Los Ask son rpc


    [PunRPC]
    void AsktoMoveTetraSide(Vector3 dir,Player p ) {
        if ( _view.IsMine ) {
            return;
        }
        if ( managers.ContainsKey(p) ) {
            managers [ p ].MoveTetraHorizontal(dir);
            Debug.Log("moviendo");
        }
    }

    [PunRPC]
    void AskNewTetraIndex( Player p, int r ) {
        if ( _view.IsMine ) {
            return;
        }
        if ( managers.ContainsKey(p) ) {
            managers [ p ].NewTetraIndex(r);
        }
    }

    [PunRPC]
    void AskNewTetra( Player p ) {
        if ( _view.IsMine ) return;
        if ( managers.ContainsKey(p) ) {
            managers [ p ].NewTetra();
        }
    }

    [PunRPC]
    void AskFactoryPosition(Player p) {
        if ( _view.IsMine ) return;

        if ( managers.ContainsKey(p) ) {
            managers [ p ].SetStartPosition();
        }

    }

    //Los request no son rpc
    public void ManagerRequestNewTetraIndex( Player p ) {
        int r = Random.Range(0, 7);
        _view.RPC("AskNewTetraIndex", server, p, r);
    }
    public void ManagerRequestNewTetra( Player p ) {
        _view.RPC("AskNewTetra", server, p);            //CREO
    }
    public void PlayerRequestMoveTetraSide(Vector3 dir, Player p ) {
        _view.RPC("AsktoMoveTetraSide", server, dir, p);
    }
    public void ManagerRequestFactotyPosition( Player p ) {
        _view.RPC("AskFactoryPosition", server, p);
    }
    public void ManagerRequestToStart( Player p ) {
        _view.RPC("AskFactoryPosition", p, p);
        _view.RPC("AskFactoryPosition", server, p);
        _view.RPC("AskFactoryPosition", RpcTarget.All, p);

        /*
        int r = Random.Range(0, 7);
        _view.RPC("AskNewTetraIndex", p, p, r);
        _view.RPC("AskNewTetra", p, p);            //CREO
        _view.RPC("AskNewTetraIndex", RpcTarget.All, p, r);
        _view.RPC("AskNewTetra", RpcTarget.All, p);            //CREO
        */


    }

}
