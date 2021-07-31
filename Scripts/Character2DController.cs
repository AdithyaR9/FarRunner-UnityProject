using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character2DController : MonoBehaviour
{
    //public variables
        //References
            //Prefabs
    public GameObject projectileSinglePrefab;
    public GameObject projectileMultiPrefab;
            //HealthBar
    public HealthBar healthBar;
            //Orientation Sprites
    public Sprite rightOrientationSprite;
    public Sprite leftOrientationSprite;
        //Moving
    public float movementSpeed = 3f;
    public float jumpForce = 5f;
    public bool inSlowLiquid;
    public float slowLiquidRate;
        //Health
    public static int maxHealth;
    public int inspectorMaxHealth;   //To See and Set Max Health in Inspector
    public static int currentHealth;
    public int enemyDamageTaken;
    public int obstacleDamageTaken;
        //Shooting
    public float projectileSpeedSingle = 10f;
    public float projectileFiringPeriodSingle = 0.3f;
    public float projectileSpeedMulti = 15f;
    public float projectileFiringPeriodMulti = 0.5f;
    public float damageEffectDuration;
    public static bool hasMultiShot;
        //Player State
    public bool isDead;
    public bool gameWon;

    //private variables
    private float savedMovementSpeed;
        //Cached References
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Coroutine firingCoroutineSingle;
    private Coroutine firingCoroutineMulti;


    private void Awake()
    {
        maxHealth = inspectorMaxHealth;       
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        hasMultiShot = false;
        inSlowLiquid = false;
        savedMovementSpeed = movementSpeed;
        healthBar.SetMaxHealth(maxHealth);
        isDead = false;
        gameWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthCheck();
        IsGameWon();

        if (!gameWon && !isDead)
        {
            Movement();

            FireSingle();
            if (hasMultiShot)
            {
                FireMulti();
            }
        }


    }

    //Checks if Dead
    private void HealthCheck()
    {
        if (currentHealth <= 0)
        {
            spriteRenderer.color = Color.black;
            healthBar.SetHealth(currentHealth);
            isDead = true;

            FindObjectOfType<GameManager>().EndGame(1);
        }
        else {
            healthBar.SetHealth(currentHealth);
        }
    }

    //Has the Player Reached the End
    private void IsGameWon()
    {
        if (gameWon)
        {
            FindObjectOfType<GameManager>().EndGame(0);
        }
    }

    //Does Player Movment and Jumping
    private void Movement()
    {
        var movement = Input.GetAxis("Horizontal");
        SpriteOrientation(movement);

        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * movementSpeed;

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rigidbody.velocity.y) < 0.001f)
        {
            rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
    private void SpriteOrientation(float movement)
    {
        if (movement == 1)
        {
            spriteRenderer.sprite = rightOrientationSprite;
            projectileSpeedSingle = Mathf.Abs(projectileSpeedSingle);
            projectileSpeedMulti = Mathf.Abs(projectileSpeedMulti);

        }
        else if (movement == -1)
        {
            spriteRenderer.sprite = leftOrientationSprite;
            projectileSpeedSingle = -1 * Mathf.Abs(projectileSpeedSingle);
            projectileSpeedMulti = -1 * Mathf.Abs(projectileSpeedMulti);
        }
    }


    //When Firing Projectiles
    private void FireSingle()   // Fires Single-Shots
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutineSingle = StartCoroutine(FireContinuouslySingle());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutineSingle);
        }
    }
    IEnumerator FireContinuouslySingle()
    {
        //Vector3 projectilePosition = transform.position + new Vector3(1, 0, 0); 

        while (true)
        {
            GameObject projectile = Instantiate(projectileSinglePrefab,
               transform.position, Quaternion.identity) as GameObject;
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeedSingle, 0);
            yield return new WaitForSeconds(projectileFiringPeriodSingle);
        }
    }
    private void FireMulti()         //Fires Multi-Shots
    {
        if (Input.GetButtonDown("Fire2"))
        {
            firingCoroutineMulti = StartCoroutine(FireContinuouslyMulti());
        }
        if (Input.GetButtonUp("Fire2"))
        {
            StopCoroutine(firingCoroutineMulti);
        }
    }
    IEnumerator FireContinuouslyMulti()
    {
        //Vector3 projectilePosition = transform.position + new Vector3(1, 0, 0);

        while (true)
        {
            //Top Shot
            GameObject projectile1 = Instantiate(projectileMultiPrefab,
               transform.position, Quaternion.identity) as GameObject;
            projectile1.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeedMulti, projectileSpeedMulti / 4);
            //Middle Shot
            GameObject projectile2 = Instantiate(projectileMultiPrefab,
               transform.position, Quaternion.identity) as GameObject;
            projectile2.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeedMulti, 0);
            //Bottom Shot
            GameObject projectile3 = Instantiate(projectileMultiPrefab,
               transform.position, Quaternion.identity) as GameObject;
            projectile3.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeedMulti, (-1 * projectileSpeedMulti)/4);

            yield return new WaitForSeconds(projectileFiringPeriodMulti);
        }
    }
    public static void TurnOnMultiShot()
    {
        hasMultiShot = true;
    }

    //When Interacting w/ Other Collider Rigidbodies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (inSlowLiquid && collision.gameObject.CompareTag("Ground"))
        {
            inSlowLiquid = false;
            movementSpeed = savedMovementSpeed;
        }

        if (!isDead)
        {
            EnemyObstacleCollision(collision);
        }
    }
    private void EnemyObstacleCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            TakeDamage(enemyDamageTaken);

            //Damage Effect
            spriteRenderer.color = Color.red;
            Invoke("RevertColor", damageEffectDuration);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            TakeDamage(obstacleDamageTaken);

            //Damage Effect
            spriteRenderer.color = Color.red;
            Invoke("RevertColor", damageEffectDuration);
        }
    }
    private void TakeDamage(int damageType)
    {
        currentHealth -= damageType;
        healthBar.SetHealth(currentHealth);
    }
    private void RevertColor()
    {
        spriteRenderer.color = Color.white;
    }


    //When Interacting w/ Other Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IsInWater(collision);
    }

    private void IsInWater(Collider2D collision)
    {
        if (!inSlowLiquid && collision.gameObject.CompareTag("SlowLiquid"))
        {
            inSlowLiquid = true;
            movementSpeed *= slowLiquidRate;
        }
    }
}
