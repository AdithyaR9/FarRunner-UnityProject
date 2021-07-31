using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShotPowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character2DController.TurnOnMultiShot();
            Destroy(gameObject);
        }
    }
}
