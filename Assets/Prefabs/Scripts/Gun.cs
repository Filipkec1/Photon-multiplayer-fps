using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 20f;

    private float nextTimeToFire = 0f;
    public bool isAutomatic = true;

    public int maxAmmo = 10;
    public int currentAmmo;
    public float reloadTime = 1f;
    public bool isReloading = false;

    public LayerMask player;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public Animator animator;
    public AudioSource audioSourceFire;

    //network
    private PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }


    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        if (PouseMenue.isGamePoused)
        {
            return;
        }

        if (isReloading)
        {
            return;
        }

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Reload") && !(currentAmmo == maxAmmo))
        {
            StartCoroutine(Reload());
        }

        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && (Time.time >= nextTimeToFire))
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                PV.RPC("RPC_Shoot", RpcTarget.All);
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && (Time.time >= nextTimeToFire))
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                PV.RPC("RPC_Shoot", RpcTarget.All);
            }
        }

    }

    string remotePlayerLayer = "RemotePlayer";

    [PunRPC]
    void RPC_Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;
        audioSourceFire.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, player))
        {

            Target target = hit.transform.GetComponent<Target>();

            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(remotePlayerLayer))
            {
                Debug.Log("TARGET HIT");
            }

            if(target != null)
            {
                target.PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
                //target.TakeDamage(damage);
                //PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharecter);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject  impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 0.5f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime-0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
