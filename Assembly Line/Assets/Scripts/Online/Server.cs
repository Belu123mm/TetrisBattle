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
    
    // Start is called before the first frame update
    
    void Awake()
    {
        _view = GetComponent<PhotonView>();


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
        foreach ( var item in assemblers ) {
            Debug.Log(item);
        }
    }

    //Los Ask son rpc

    [PunRPC]
    public void AskAssemblerToMoveHorizontal(Vector3 dir,Player p ) {
        if ( _view.IsMine ) {
            return;
        }
        if ( assemblers.ContainsKey(p) ) {
            assemblers [ p ].MoveHorizontal(dir);
        }
    }
    [PunRPC]
    public void AskAssemblerToMoveVertical( Vector3 dir, Player p ) {
        Debug.Log("movingvertical1");

        Debug.Log(assemblers.ContainsKey(p));
        if ( _view.IsMine ) {
            return;
        }
        Debug.Log("movingvertical2");
        if ( assemblers.ContainsKey(p) ) {
            Debug.Log("movingvertical3");
            assemblers [ p ].MoveVertical(dir);
        }
    }
    [PunRPC]
    public void AskAssemblerToGrab(Player p ) {
        if ( _view.IsMine ) {
            return;
        }
        if ( assemblers.ContainsKey(p) ) {
            Debug.Log("grabbin");
            assemblers [ p ].Grab();
        }


    }
    /*

[PunRPC]
void AskNewTetraIndex( Player p, int r ) {
    if ( _view.IsMine ) {
        return;
    }
    if ( assemblers.ContainsKey(p) ) {
        assemblers [ p ].NewTetraIndex(r);
    }
}

[PunRPC]
void AskNewTetra( Player p ) {
    if ( _view.IsMine ) return;
    if ( assemblers.ContainsKey(p) ) {
        assemblers [ p ].NewTetra();
    }
}

[PunRPC]
void AskFactoryPosition(Player p) {
    if ( _view.IsMine ) return;

    if ( assemblers.ContainsKey(p) ) {
        assemblers [ p ].SetStartPosition();
    }

}
*/

    //Los request no son rpc
    public void PlayerRequestToMoveHorizontal(Vector3 dir,Player p ) {
        _view.RPC("AskAssemblerToMoveHorizontal", RpcTarget.AllViaServer, dir, p);
    }
    public void PlayerRequestToMoveVertical( Vector3 dir, Player p ) {
        _view.RPC("AskAssemblerToMoveVertical", RpcTarget.AllViaServer, dir, p);
    }
    public void PlayerRequestToGrab(Player p ) {
        _view.RPC("AskAssemblerToMoveVertical", RpcTarget.AllViaServer, p);
    }







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
