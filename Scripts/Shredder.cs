using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{

    public Transform player;
    public float aheadDistance;


    void Update()
    {
        transform.position = new Vector3(player.position.x + aheadDistance, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectileSingle") || collision.gameObject.CompareTag("ProjectileMulti")) 
        {
            Destroy(collision.gameObject);
        }
    }


}
