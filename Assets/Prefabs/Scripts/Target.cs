using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;
    public float currentHealth;

    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        set { _isDead = value; }
    }

    public AvatarSetup avatarSetup;

    public PhotonView PV;

    void Start()
    {
        avatarSetup = GetComponent<AvatarSetup>();
        PV = GetComponent<PhotonView>();
        currentHealth = health;
    }


    [PunRPC]
    public void RPC_TakeDamage(float amount)
    {

        if (isDead)
        {
            return;
        }

        currentHealth -= amount;
        if(currentHealth <= 0f)
        {
            PV.RPC("RPC_Die", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_Die()
    {
        isDead = true;

        Debug.Log("Player DIED!");

        //Disable components
        for (int i = 0; i < avatarSetup.toReanable.Length; i++)
        {
            avatarSetup.toReanable[i].enabled = false;
        } 

        //Disable GameObjects
        for (int i=0; i< avatarSetup.disableGameObjectsOnDeath.Length; i++)
        {
            avatarSetup.disableGameObjectsOnDeath[i].SetActive(false);
        }

        avatarSetup.mycharacter.SetActive(false);
        //Destroy(avatarSetup.mycharacter);

        if (PV.IsMine)
        {
            GameSetuoController.GSC.SetSceenCameraActive(true);
            currentHealth = health;
        }

        StartCoroutine(avatarSetup.Respawn());

    }
}
