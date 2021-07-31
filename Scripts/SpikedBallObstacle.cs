using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBallObstacle : MonoBehaviour
{
    //public
    public float xForce;
    public float yForce;
    public float startPosX;
    public float movementDistance;
    public float counterForce;
    public float damageEffectDuration;

    //private
    private float directionalXForce;

    // Cached References
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectileSingle") || collision.gameObject.CompareTag("ProjectileMulti"))
        {
            //Destroy Projectile
            Destroy(collision.gameObject);

            // Damage Effect
            spriteRenderer.color = Color.black;
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
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.position = new Vector3(startPosX, transform.position.y, transform.position.y);
        directionalXForce = xForce;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (directionalXForce == xForce && (startPosX + movementDistance) <= transform.position.x)
        {
            directionalXForce *= -1;
            rigidbody.AddForce(new Vector2(directionalXForce * counterForce, yForce));
        }
        else if (directionalXForce == (-1 * xForce) && (startPosX - movementDistance) >= transform.position.x)
        {
            directionalXForce *= -1;
            rigidbody.AddForce(new Vector2(directionalXForce * counterForce, yForce));
        }
        else if ((startPosX - movementDistance) < transform.position.x && transform.position.x < (startPosX + movementDistance))
        {
            rigidbody.AddForce(new Vector2(directionalXForce, yForce));
        }
    }

}
