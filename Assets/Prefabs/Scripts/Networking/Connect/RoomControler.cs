using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;

public class RoomControler : MonoBehaviourPunCallbacks, IInRoomCallbacks
{

    public static RoomControler room;
    private PhotonView PV;

    public int currentScene;

    [SerializeField]
    private int multiplayerScreneIndex;

    private void Awake()
    {
        
        //set up singleton
        if(RoomControler.room == null)
        {
            RoomControler.room = this;
        }
        else{

            if(RoomControler.room != this)
            {
                Destroy(RoomControler.room.gameObject);
                RoomControler.room = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");
        PhotonNetwork.AutomaticallySyncScene = true;
        StartGame();
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting game");
            PhotonNetwork.LoadLevel(multiplayerScreneIndex);
        }
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if(currentScene == multiplayerScreneIndex)
        {
            CreatePlayer();
        }
    }

    void CreatePlayer()
    {
        Debug.Log("Room create player");

        PhotonNetwork.Instantiate(Path.Combine("PlayerFolder", "PlayerAvatar"),
                transform.position, Quaternion.identity, 0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
    }
}
