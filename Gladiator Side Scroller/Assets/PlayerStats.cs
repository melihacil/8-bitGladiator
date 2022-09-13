using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    private HealthScript health;
    [SerializeField] private float healthVal;

    public bool isInvulnerable;
    public bool isDead;

    private void Awake()
    {
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
            GetComponentInChildren<PlayerMovement>().Death();
            isInvulnerable = true;
            isDead = true;
        }

    }


}
