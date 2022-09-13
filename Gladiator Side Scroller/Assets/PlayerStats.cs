using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Script to hold player values 
    private HealthScript health;
    [SerializeField] private float healthVal;
    [SerializeField] private GameObject deathPanel;


    public bool isInvulnerable;

    private void Awake()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
        health = GetComponent<HealthScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health.SetSliderVal(healthVal);
    }

    public void DamagePlayer(float damage)
    {
        if (isInvulnerable)
            return;
        healthVal -= damage;
        health.UpdateSlider(healthVal);
        if (healthVal <= 0)
        {
            if (deathPanel != null)
                deathPanel.SetActive(true);
            GetComponentInChildren<PlayerMovement>().Death();
            isInvulnerable = true;
        }

    }


}
