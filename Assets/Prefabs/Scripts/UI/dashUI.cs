using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashUI : MonoBehaviour
{

    public RectTransform dashReset;

    public PlayerMovment playerMovment;

    void Start()
    {
        playerMovment.GetComponent<PlayerMovment>();
    }

    void Update()
    {
        SetDesh (playerMovment.GetDashRegen());
    }

    void SetDesh(float _amount)
    {
        dashReset.localScale = new Vector3(_amount, 1f, 1f);
    }


}
