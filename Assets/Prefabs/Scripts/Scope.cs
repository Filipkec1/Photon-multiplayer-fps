using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
    public bool isScoped = false;
    private float scopeInTime = 0.15f;

    public Animator animator;
    public GameObject crosshairOcerlay;
    public Gun gun;

    public bool reload;

    void Start()
    {
        gun.GetComponent<Gun>();
    }

    void Update()
    {

        if (PouseMenue.isGamePoused)
        {
            return;
        }

        if (gun.isReloading)
        {
            isScoped = false;
            animator.SetBool("Scoping", isScoped);
            StartCoroutine(OnUnscope());
            return;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            isScoped = !isScoped;


            animator.SetBool("Scoping", isScoped);

            if (isScoped)
            {
                StartCoroutine(OnScoped());
            }
            else
            {
                StartCoroutine(OnUnscope());
            }
        }     
    }

    IEnumerator OnUnscope()
    {
        yield return new WaitForSeconds(scopeInTime);
        crosshairOcerlay.SetActive(true);
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(scopeInTime);
        crosshairOcerlay.SetActive(false);
    }


}
