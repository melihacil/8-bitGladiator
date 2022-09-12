using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    private HealthScript health;
    [SerializeField] private float healthVal;

    public bool isInvulnerable;


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

    }


}
