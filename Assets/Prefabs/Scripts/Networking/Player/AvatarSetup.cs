using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;

    public GameObject mycharacter;

    public int characterValue;

    public Target target;
    public WeaponSwap weaponSwap;
    private Gun gun;

    [SerializeField]
    Behaviour[] componentsToDisable;
    
    public GameObject[] disableGameObjectsOnDeath;
    public Behaviour[] toReanable;

    bool firstSpawn = true;

    void Start()
    {
        target = GetComponent<Target>();
        weaponSwap = GetComponent<WeaponSwap>();

        PV = GetComponent<PhotonView>();
        SetupPlayer();
    }

    public void SetupPlayer()
    {
        if (PV.IsMine)
        {

            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharecter);
            GameSetuoController.GSC.SetSceenCameraActive(false);
        }
        else
        {
            AssignRemoteLayer();
            DisableComponents();
        }
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    void AssignRemoteLayer()
    {

        SetLayerRecursively(gameObject, LayerMask.NameToLayer(remoteLayerName));
        //gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }


    [SerializeField]
    string dontDrawLayerName = "Pass";

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        if (firstSpawn)
        {
            mycharacter = Instantiate(PlayerInfo.PI.allCharacters[whichCharacter], transform.position, transform.rotation, transform);
            SetLayerRecursively(mycharacter, LayerMask.NameToLayer(dontDrawLayerName));
            firstSpawn = false;
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameSetuoController.GSC.respawnTime);

        //Enable components
        for (int i = 0; i < toReanable.Length; i++)
        {
            toReanable[i].enabled = true;
        }

        //Enable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        target.currentHealth = target.health;
        target.isDead = false;

        yield return new WaitForSeconds(0.1f);
        SetupPlayer();
        mycharacter.SetActive(true);
    }


}
