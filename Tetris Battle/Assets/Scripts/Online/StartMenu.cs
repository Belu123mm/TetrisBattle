using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviourPunCallbacks {
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
        
        Debug.Log("disconectto" + playerCount);
        PhotonNetwork.Disconnect();
    }

    public void EnterToServer() {
        //Crear un nuevo server yluego pasar a la escena del juego, usando la cantidad de players que esta ahi
        PhotonNetwork.ConnectUsingSettings();


    }
    public override void OnConnectedToMaster() {
        conectionState.text = "CONECTADO AL SERVER";
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnDisconnected( DisconnectCause cause ) {
        conectionState.text = cause.ToString();
    }

    public override void OnJoinedLobby() {
        PhotonNetwork.AutomaticallySyncScene = true;
        conectionState.text = "EN EL LOBBY";

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
        //recien cuando hay gente los tiras a la room?



    }

    public override void OnJoinedRoom() {
        conectionState.text = "EN LA HABITACION";

        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + " ndeah");
        /*      //Esto no va aca, pero acordate de esto
        if ( isHost )
                    PhotonNetwork.Instantiate("Server7w7", Vector3.zero, Quaternion.identity);
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
                        Debug.Log("CHANGING SCENE REEEEEEE");
                        PhotonNetwork.LoadLevel(playerCount - 2);
                        inGame = true;
                        return;
                    }
                }
            }//si no sos el host, no haces nada
        } else {//ahora, si hay juego, pues creas el server o el pj
            if ( isHost == true ) {
                Debug.Log("es host y hay juego");
                if ( !isServerOn ) {
                    Debug.Log("Server creado");
                    isServerOn = true;
                    PhotonNetwork.Instantiate("Server7w7", Vector3.right * 2, Quaternion.identity);
                    return;
                }
            } else {
                if ( isPlayerOn == false) {
                    Debug.Log("player creado");
                    isPlayerOn = true;
                    PhotonNetwork.Instantiate("Person", Vector3.zero, Quaternion.identity);
                    return;
                }
            }
        }


    }
}
