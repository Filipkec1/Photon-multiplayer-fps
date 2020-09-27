using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthUI : MonoBehaviour
{
    public Target target;

    Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        target.GetComponent<Target>();
    }

    void Awake()
    {
        healthText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = target.currentHealth + " / " + target.health;
    }
}
