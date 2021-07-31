using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceEnemy : MonoBehaviour
{
    //public
    public float groundPosY;
    public float heightAboveGroundY;     // for future reference for x point
    public float fallSpeed;
    public float riseSpeed;
    public float health;
    public float projectileSingleDamage;
    public float projectileMultiDamage;
    public float damageEffectDuration;
    public int hitScoreValue;
    public int killedScoreValue;

    //private
    private float posX;
    private float startPosY;
    private float finalPosY;
    private float finalHeightAdjustment = 0.75f;
    private bool isFalling;

    //Cached References
    private SpriteRenderer spriteRenderer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((collision.gameObject.transform.position.x <= transform.position.x + 1) && (collision.gameObject.transform.position.x >= transform.position.x - 1)) && collision.gameObject.transform.position.y <= transform.position.y)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Character2DController.currentHealth = -1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectileSingle"))
        {
            //Enact Damage, Update Player Score, Destroy Projectile
            health -= projectileSingleDamage;
            Score.UpdateScore(hitScoreValue);
            Destroy(collision.gameObject);

            // Damage Effect
            spriteRenderer.color = Color.red;
            Invoke("RevertColor", damageEffectDuration);
        }
        else if (collision.gameObject.CompareTag("ProjectileMulti"))
        {
            //Enact Damage, Update Player Score, Destroy Projectile
            health -= projectileMultiDamage;
            Score.UpdateScore(hitScoreValue);
            Destroy(collision.gameObject);

            // Damage Effect
            spriteRenderer.color = Color.red;
            Invoke("RevertColor", damageEffectDuration);
        }
    }
    private void RevertColor()
    {
        spriteRenderer.color = Color.white;
    }


    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        startPosY = groundPosY + heightAboveGroundY;
        finalPosY = groundPosY + finalHeightAdjustment;
        isFalling = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = new Vector3(posX, startPosY, transform.position.z);
        //transform.position = wayPointsXY[waypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();

        if (transform.position.y <= finalPosY || transform.position.y >= startPosY)
        {
            isFallingCheck();
        }

        Move();

    }

    private void HealthCheck()
    {
        if (health <= 0)
        {
            //Update Score, Self-Destroy
            Score.UpdateScore(killedScoreValue);
            Destroy(gameObject);
        }
    }

    private void isFallingCheck()
    {
        if (isFalling && transform.position.y <= finalPosY)
        {
            isFalling = false;
        }
        else if (!isFalling && transform.position.y >= startPosY)
        {
            isFalling = true;
        }
    }

    private void Move()
    {
        if (isFalling && transform.position.y != finalPosY)
        {
            transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * fallSpeed;
        }
        else if (!isFalling && transform.position.y != startPosY)
        {
            transform.position += new Vector3(0, 1, 0) * Time.deltaTime * riseSpeed;
        }
    }
}
