using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviourPunCallbacks {    //Todo esto se ejecuta localmente, aca dberia ir lo de las camaras... creo 
    public TMP_Text conectionState;
    public TMP_Dropdown dropdown;
    public bool isHost;
    public int playerCount;
    public bool inGame;
    public bool isServerOn;
    public bool isPlayerOn;

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
        PhotonNetwork.Disconnect();
    }

    public void EnterToServer() {
        //Crear un nuevo server yluego pasar a la escena del juego, usando la cantidad de players que esta ahi
        PhotonNetwork.ConnectUsingSettings();


    }
    public override void OnConnectedToMaster() {
        conectionState.text = "Connecting to Server";
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected( DisconnectCause cause ) {
        conectionState.text = cause.ToString();
    }

    public override void OnJoinedLobby() {
        PhotonNetwork.AutomaticallySyncScene = true;
        conectionState.text = "In Lobby";

        byte b = 0;
        if ( dropdown.value == 0 ) b = 3;
        else if ( dropdown.value == 1 ) b = 4;
        else if (dropdown.value == 2 ) b = 5;

        
        if ( isHost ) {
            //Esta es la instancia del juego, en terminos de network no de escenas 
            PhotonNetwork.CreateRoom("MainRoom", new RoomOptions() { MaxPlayers = b });
            return;
        }
        else {
        PhotonNetwork.JoinRandomRoom();

        }
    }

    public override void OnJoinedRoom() {
        conectionState.text = "In room";

    }

    public override void OnJoinRandomFailed( short returnCode, string message ) {
        conectionState.text = "Failed. Cause: " + message;
        PhotonNetwork.Disconnect();
    }

    public override void OnCreatedRoom() {
        conectionState.text = "Room created";
    }
    private void Update() {
        //Esto es para cambiar de escena y spawnear los players
        if (SceneManager.GetActiveScene().name != "Lobby" ) {
            inGame = true;
        }


        if ( inGame == false) {
            if ( isHost ) {
                if ( PhotonNetwork.InRoom ) {//Y se cumple esta cosa cambias de escena
                    playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

                    if ( playerCount == PhotonNetwork.CurrentRoom.MaxPlayers ) {
                        //PhotonNetwork.LoadLevel(playerCount - 2);

                        PhotonNetwork.LoadLevel("Game");
                        inGame = true;
                        return;
                    }
                }
            }//si no sos el host, no haces nada
        } else {//ahora, si hay juego, pues creas el server o el pj
            if ( isHost == true ) {

                if ( !isServerOn ) {
                    isServerOn = true;
                    PhotonNetwork.Instantiate("Server", Vector3.zero, Quaternion.identity);
                    return;
                }
            } else {
                if ( isPlayerOn == false) {
                    isPlayerOn = true;
                    PhotonNetwork.Instantiate("Controller", Vector3.zero, Quaternion.identity);
                    return;
                }
            }
        }


    }
}
