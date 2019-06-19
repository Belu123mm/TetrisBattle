using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class StartMenu : MonoBehaviourPunCallbacks {
    public TMP_Text conectionState;
    public TMP_Dropdown dropdown;
    bool isHost;
    int playerCount;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    private void Start() {
        DontDestroyOnLoad(this);
    }
    public void HostServer() {
        isHost = true;
        PhotonNetwork.ConnectUsingSettings();
        conectionState.gameObject.SetActive(true);
    }

    public void Disconnect() {
        playerCount--;
        Debug.Log("disconectto" + playerCount);
        PhotonNetwork.Disconnect();
    }

    public void EnterToServer() {
        //Crear un nuevo server yluego pasar a la escena del juego, usando la cantidad de players que esta ahi
        PhotonNetwork.ConnectUsingSettings();
        conectionState.gameObject.SetActive(true);


    }
    public override void OnConnectedToMaster() {
        playerCount++;
        conectionState.text = "CONECTADO AL SERVER";
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected( DisconnectCause cause ) {
        conectionState.text = cause.ToString();
    }

    public override void OnJoinedLobby() {
        conectionState.text = "EN EL LOBBY";

        byte b = 0;
        if ( dropdown.value == 0 ) b = 3;
        else if ( dropdown.value == 1 ) b = 4;
        else if (dropdown.value == 2 ) b = 5;

        
        if ( isHost ) {
            //Esta es la instancia del juego, en terminos de network no de escenas 
            PhotonNetwork.CreateRoom("MainRoom", new RoomOptions() { MaxPlayers = b });
        }

        //recien cuando hay gente los tiras a la room?


        PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinedRoom() {
        conectionState.text = "EN LA HABITACION";

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " ndeah");

        ChangeScene();      //Right?
        //only the master client may load the level

        if ( isHost )
            PhotonNetwork.Instantiate("Server7w7", Vector3.zero, Quaternion.identity);
        /*
        else
            PhotonNetwork.Instantiate("Person", Vector3.zero, Quaternion.identity);
        */
    }
    //public override void update

    public override void OnJoinRandomFailed( short returnCode, string message ) {
        conectionState.text = "FALLO POR QUE " + message;
        PhotonNetwork.Disconnect();
    }

    public override void OnCreatedRoom() {
        conectionState.text = "CREO UNA HABITACION";
    }

    [PunRPC]
    void ChangeScene() {
        if ( PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers ) {
            Debug.Log("CHANGING SCENE REEEEEEE");
            Server.Instance.StartGame(dropdown.value + 1);
        }

    }



}
