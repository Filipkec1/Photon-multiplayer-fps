using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;

public class GameSetuoController : MonoBehaviour
{

    public static GameSetuoController GSC;

    public Transform[] spawnPoints;

    [SerializeField]
    private GameObject sceneCam;

    public float respawnTime = 3f;

    public int mainMenuIndex = 0;

    private void OnEnable()
    {
        Debug.Log("Game Setup ready");

        if(GameSetuoController.GSC == null)
        {
            GameSetuoController.GSC = this;
        }
    }

    public void SetSceenCameraActive(bool isActive)
    {
        if (sceneCam == null)
        {
            return;
        }

        sceneCam.SetActive(isActive);

    }


    public void DisconnetPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
        {
            yield return null;
        }

        SceneManager.LoadScene(mainMenuIndex);
    }
}
