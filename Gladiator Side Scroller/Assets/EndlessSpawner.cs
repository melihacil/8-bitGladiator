using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessSpawner : MonoBehaviour
{


    [SerializeField] private GameObject m_gameObject;
    [SerializeField] private Text scoreText;
    private int SpawnedCount = 2;
    
    [SerializeField] private Transform m_SpawnPoint_1;
    [SerializeField] private Transform m_SpawnPoint_2;


    private bool alreadySpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnedCount < 10 && !alreadySpawned)
        {

            Invoke(nameof(Spawn), 2f);
            alreadySpawned = true;
        }
    }

    public void ReduceCount()
    {
        SpawnedCount--;
    }


    private void Spawn()
    {
        Instantiate(m_gameObject, m_SpawnPoint_1.position, Quaternion.identity);
        Instantiate(m_gameObject, m_SpawnPoint_2.position, Quaternion.identity);
        SpawnedCount += 2;
        alreadySpawned = false;
    }

}
