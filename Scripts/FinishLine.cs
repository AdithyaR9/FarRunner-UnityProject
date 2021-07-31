using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public Character2DController player;
    public float flagHalfWidth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > gameObject.transform.position.x - flagHalfWidth)
        {
            player.gameWon = true;
        }
    }
}
