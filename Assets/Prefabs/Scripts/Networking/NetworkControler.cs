using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkControler : MonoBehaviourPunCallbacks
{
    public GameObject conncted;

    void Start()
    {
        conncted.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("Spojen na " + PhotonNetwork.CloudRegion + " server");
        conncted.SetActive(true);
        //base.OnConnected();
    }
}
