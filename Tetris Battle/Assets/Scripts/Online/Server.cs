using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

public class Server : MonoBehaviourPun {
    public static Server Instance { get; private set; } //El private es para que no te hackeen y que lo seteen(xd) de otro lado 
    public Dictionary<Player, Person> persons = new Dictionary<Player, Person>();
    PhotonView _view;
    public Player server;
    //Cuando cambia escena ahi es cuando crea los persons?
    
    // Start is called before the first frame update
    void Awake()
    {
        _view = GetComponent<PhotonView>();
        if ( !Instance ) {
            if ( _view.IsMine )
                _view.RPC("SetReferenceToSelf", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);  //Buffered: todos los que se conecten tarde reciben este mensaje

        } else
            PhotonNetwork.Destroy(gameObject);  //Me destrushe


    }

    [PunRPC]    //Funcion que se llama enel servidory se replica en todos los clientes 
    public void SetReferenceToSelf( Player p ) {    //Esto es como un singleton pero para redes
        Instance = this;
        server = p;
        
        if ( !PhotonNetwork.IsMasterClient )    //Wachin, sos el master?
            _view.RPC("AddPlayer", server, PhotonNetwork.LocalPlayer);//Flasheaste, te agrego un player
            
    }

    [PunRPC]
    public void AddPlayer( Player p ) {
        if ( !_view.IsMine )
            return;     //Wat is dis?
        var newPlayer = PhotonNetwork.Instantiate("Person",
                        new Vector3(Random.Range(0, 3),
                        Random.Range(0, 3),
                        Random.Range(0, 3)),
                        Quaternion.identity).GetComponent<Person>();
        persons.Add(p, newPlayer);
        foreach ( var item in persons ) {
            Debug.Log(item);
        }
    }

    [PunRPC] 
    public void StartGame(int value) {
        PhotonNetwork.LoadLevel(value + 1);
        PhotonNetwork.AutomaticallySyncScene = true;    //For some reason,this only happends in the last 

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
/*    [PunRPC]
    void ChangeScene() {
        if ( PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers ) {
            Debug.Log("CHANGING SCENE REEEEEEE");
            PhotonNetwork.AutomaticallySyncScene = false;    //For some reason,this only happends in the last 
            PhotonNetwork.LoadLevel(dropdown.value + 1);
        }

    }
*/