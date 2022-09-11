using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthScript))]
public class EnemyStats : MonoBehaviour
{
    //[SerializeField] private Transform playerPos;
    [SerializeField] private HealthScript healthScript;
    [SerializeField] private float enemyHealth;
    public bool isDead { get; private set; }

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
        //transform.position = playerPos.position;
        if (isDead)
            Invoke(nameof(DestroyGameObject), 4f);
    }

    public void UpdateHealth(float damage)
    {
        enemyHealth -= damage;
        healthScript.UpdateSlider(enemyHealth);
        if (enemyHealth <= 0)
            isDead = true;
    }



    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
