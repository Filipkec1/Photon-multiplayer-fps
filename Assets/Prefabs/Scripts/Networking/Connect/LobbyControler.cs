using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyControler: MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject startButtonMenue;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject cancelButtonMenue;
    [SerializeField]
    private GameObject cancelButton;
    [SerializeField]
    private int RoomSize;

    public override void OnConnectedToMaster()
    {
        startButton.SetActive(true);
    }

    public void QuickStart()
    {
        startButton.SetActive(false);
        startButtonMenue.SetActive(false);
        cancelButton.SetActive(true);
        cancelButtonMenue.SetActive(true);

        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Quick start");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Faild to join a room");
        CreateRoom();
    }

    void CreateRoom()
    {
        Debug.Log("Creating room now");
        int randomRoomNumber = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)RoomSize };
        PhotonNetwork.CreateRoom("Room " + randomRoomNumber, roomOps);
        Debug.Log("Room " + randomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room");
        CreateRoom();
    }

    public void QuickCancel()
    {
        startButton.SetActive(true);
        startButtonMenue.SetActive(true);
        cancelButton.SetActive(false);
        cancelButtonMenue.SetActive(false);
        PhotonNetwork.LeaveRoom();
    }

}
