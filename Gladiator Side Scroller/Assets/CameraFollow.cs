using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform player;



    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, offset.z);
    }


}
