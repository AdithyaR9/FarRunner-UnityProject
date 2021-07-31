using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int collectableScoreValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Update Score, Self-Destroy
            Score.UpdateScore(collectableScoreValue);
            Destroy(gameObject);
        }
    }

}
