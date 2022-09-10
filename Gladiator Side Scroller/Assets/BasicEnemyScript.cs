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



    public void DamageBackwards()
    {
        Debug.Log("Damaging");
        rb.AddForce(Vector2.right * 5,ForceMode2D.Impulse);
    }
}
