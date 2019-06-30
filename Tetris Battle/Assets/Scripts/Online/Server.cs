using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class Server : MonoBehaviourPun {
    public static Server Instance { get; private set; } //El private es para que no te hackeen y que lo seteen(xd) de otro lado 
    public Dictionary<Player, GameManager> managers = new Dictionary<Player, GameManager>();
    PhotonView _view;
    public Player server;
    public Queue<Vector3> gmpositions = new Queue<Vector3>();
    
    // Start is called before the first frame update
    
    void Awake()
    {
        Debug.Log(PhotonNetwork.IsMasterClient + "es maestro wey?");
        Debug.Log("aqui estoy");
        _view = GetComponent<PhotonView>();
        Debug.Log(_view);

        if ( !Instance ) {
            Debug.Log("no hay instancia");
            if ( _view.IsMine ) {
                _view.RPC("SetReferenceToSelf", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);  //Buffered: todos los que se conecten tarde reciben este mensaje
                Debug.Log("setreference con rpc");
            }

        }
        else {
            Debug.Log("adios");     //nunca entra aca pero igual se destruye el server? 
            PhotonNetwork.Destroy(gameObject);  //Me destrushe

        }

        gmpositions.Enqueue(Vector3.zero);
        gmpositions.Enqueue(Vector3.right * 5);
        gmpositions.Enqueue(Vector3.right * 10);
        gmpositions.Enqueue(Vector3.right * 15); //-dab-

    }

    [PunRPC]    //Funcion que se llama enel servidory se replica en todos los clientes 
    public void SetReferenceToSelf( Player p ) {    //Esto es como un singleton pero para redes
        Debug.Log("seteando referencia de myself");
        Instance = this;
        server = p;

        DontDestroyOnLoad(this);
        if ( !PhotonNetwork.IsMasterClient )    //Wachin, sos el master?
            {
            Debug.Log("agregando manager");
            _view.RPC("AddPlayer", server, PhotonNetwork.LocalPlayer);//Flasheaste, te agrego un player
        }
            
    }

    [PunRPC]
    public void AddPlayer( Player p ) {
        Debug.Log("agrego manager");
        if ( !_view.IsMine )
            return;     //Wat is dis?
        Debug.Log(p.ActorNumber + " numerito");
        //Aca tengo que poner el tema ese de visited o no
        Vector3 newpos = gmpositions.Peek();
        var newPlayer = PhotonNetwork.Instantiate("GameManager", Vector3.right * p.ActorNumber * 25,
                        Quaternion.identity).GetComponent<GameManager>();
        //Te menti, aca hago lo de la camara

        foreach ( var item in managers ) {
            float n = 1 / (PhotonNetwork.CurrentRoom.MaxPlayers - 2);
            Debug.Log(item);
            Rect newrect = new Rect(0.5f, (p.ActorNumber - 1) * n, 0.5f, n);
        }
        managers.Add(p, newPlayer);
    }
    private void OnDestroy() {
        Debug.Log("ME DESTRUSHEN");
    }

    // Update is called once per frame
    void Update()
    {/* maybe this goes here?
        if ( PhotonNetwork.PlayerList.Length == 3 )
            PhotonNetwork.LoadLevel(1);
        if ( PhotonNetwork.PlayerList.Length == 4 )
            PhotonNetwork.LoadLevel(2);
        if ( PhotonNetwork.PlayerList.Length == 5 )
            PhotonNetwork.LoadLevel(3);
            */
    }
}
