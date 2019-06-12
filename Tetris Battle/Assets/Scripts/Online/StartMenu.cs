using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class StartMenu : MonoBehaviourPunCallbacks {
    public TMP_Text conectionState;

    bool isHost;

    private void Start() {
        DontDestroyOnLoad(this);
    }

    public void HostServer() {
        isHost = true;
        PhotonNetwork.ConnectUsingSettings();
        conectionState.gameObject.SetActive(true);

    }

    public void Disconnect() {
        PhotonNetwork.Disconnect();
    }

    public void EnterToServer() {
        //Crear un nuevo server yluego pasar a la escena del juego, usando la cantidad de players que esta ahi
        PhotonNetwork.ConnectUsingSettings();
        conectionState.gameObject.SetActive(true);


    }
    public override void OnConnectedToMaster() {
        conectionState.text = "CONECTANDO AL SERVER";
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected( DisconnectCause cause ) {
        conectionState.text = cause.ToString();
    }

    public override void OnJoinedLobby() {
        conectionState.text = "EN EL LOBBY";

        if ( isHost ) {
            //Esta es la instancia del juego, en terminos de network no de escenas 
            PhotonNetwork.CreateRoom("MainRoom", new RoomOptions() { MaxPlayers = 5 });
            return;
        }
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom() {
        conectionState.text = "EN LA HABITACION";
        //PhotonNetwork.LoadLevel(1);
        /*
        if ( isHost )
            PhotonNetwork.Instantiate("ServerNetwork", Vector3.zero, Quaternion.identity);
        else
            PhotonNetwork.Instantiate("Controller", Vector3.zero, Quaternion.identity);
        */
    }

    public override void OnJoinRandomFailed( short returnCode, string message ) {
        conectionState.text = "FALLO POR QUE " + message;
        PhotonNetwork.Disconnect();
    }

    public override void OnCreatedRoom() {
        conectionState.text = "CREO UNA HABITACION";
    }



}
