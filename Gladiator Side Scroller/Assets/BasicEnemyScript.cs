using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    public void DamageBackwards(Transform player)
    {
        Debug.Log("Damaging");

        rb.AddForce((transform.position - player.position) * 1.5f,ForceMode2D.Impulse);
    }
}
