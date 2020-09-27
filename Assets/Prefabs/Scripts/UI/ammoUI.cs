using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammoUI : MonoBehaviour
{

    public WeaponSwap weaponSwap;

    Text ammoText;

    private Gun gun;


    // Start is called before the first frame update
    void Awake()
    {
        ammoText = GetComponent<Text>();
    }

    void Start()
    {
        weaponSwap.GetComponent<WeaponSwap>();
       
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;

        foreach (Transform weapon in weaponSwap.transform)
        {
            if (i == weaponSwap.selectedWeapon)
            {
                gun = weaponSwap.transform.GetComponentInChildren<Gun>();
            }
            i++;
        }
        ammoText.text = gun.currentAmmo.ToString() + " / " + gun.maxAmmo.ToString();     
    }
}
