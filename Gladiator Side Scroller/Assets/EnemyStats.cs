using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthScript))]
public class EnemyStats : MonoBehaviour
{
    
    [SerializeField] private HealthScript healthScript;
    [SerializeField] private float enemyHealth;
    private bool isDead;

    private void Awake()
    {
        healthScript = GetComponent<HealthScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthScript.SetSliderVal(enemyHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            Invoke(nameof(DestroyGameObject), 4f);
    }

    public void UpdateHealth(float damage)
    {
        healthScript.SetSliderVal(healthScript.m_Slider.value - damage);
        if (healthScript.m_Slider.value <= 0)
            isDead = true;
    }



    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
