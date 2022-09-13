using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HealthScript))]
public class EnemyStats : MonoBehaviour
{
    //[SerializeField] private Transform playerPos;
    [SerializeField] private HealthScript healthScript;
    [SerializeField] private float enemyHealth;
    [SerializeField] private bool isEndless = false;

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

    }
    //Updates enemy health and checks if it has died
    public void UpdateHealth(float damage)
    {
        enemyHealth -= damage;
        healthScript.UpdateSlider(enemyHealth);
        if (enemyHealth <= 0)
        {
            if (isEndless)
            {
                FindObjectOfType<EndlessSpawner>().ReduceCount();
            }
            Invoke(nameof(DestroyGameObject), 15f);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<EnemyScript>().DeathState();
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }



    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
