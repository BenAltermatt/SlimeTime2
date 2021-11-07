using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Stats associated with the player
    public float health;
    public float speed;
    public float defense;
    public float projectileSpeed;
    public float projectileStrength;
    public float strength;
    public float swingSpeed;
    public float fireStrength;
    public float iceStrength;
    public float poisonStrength;

    // takes care of shooting projectiles
    public float projectileCooldown;
    private float timeShot;

    // takes care of melee attacks
    public float meleeCooldown;
    private float timeSwung;
    public int swingDir;

    public Rigidbody2D rb;
    public GameObject projectile;
    public Transform tr;
    public Camera cam;
    public Animator anim;

    // slash boxes
    public Animator[] slashAnimators;

    // hitboxes and hurtboxes
    public Collider2D hurtBox;
    public Collider2D[] hitBoxes;

    public const int RIGHT = 0;
    public const int DOWN = 1;
    public const int LEFT = 2;
    public const int UP = 3;

    private int lastDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
        timeShot = Time.time;
        timeSwung = Time.time;

        lastDir = DOWN;
        swingDir = DOWN;
        health = 100;

        Collider2D[] boxes = GetComponents<Collider2D>();
        hitBoxes = new Collider2D[4];
        hurtBox = boxes[5];
        for(int i = 1; i < boxes.Length - 1; i++)
        {
            hitBoxes[i - 1] = boxes[i];
            hitBoxes[i - 1].enabled = false;
        }

        Animator[] allAnimators = GetComponentsInChildren<Animator>();
        anim = allAnimators[0];
        slashAnimators = new Animator[4];
        for(int i = 1; i < allAnimators.Length; i++)
        {
            slashAnimators[i - 1] = allAnimators[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        move();
        if(Input.GetMouseButton(0))
        {
            fire();
        }
        if(Time.time - timeSwung > swingSpeed)
        {
            hitBoxes[swingDir].enabled = false;
            slashAnimators[swingDir].SetInteger("TriggeredDir", 4);
        }
        if(Input.GetKey("space"))
        {
            attack();
        }
    }

    private void move() 
    {
        int x = (int) Input.GetAxisRaw("Horizontal");
        int y = (int) Input.GetAxisRaw("Vertical");
        float magnitude = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));

        if(y!= 0)
        {
            lastDir = y + 2;
        }
        else if(x != 0)
        {
            lastDir = -1 * x + 1;
        }
        
        anim.SetInteger("Direction", lastDir);
        Debug.Log(anim.GetInteger("Direction"));
        rb.velocity = new Vector2(x * speed, y * speed);

        if(magnitude != 0)
            rb.velocity = rb.velocity / magnitude;
    }

    private void fire() {

        if(Time.time - timeShot > projectileCooldown) 
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = cam.ScreenToWorldPoint(mousePos);

            GameObject shot = Instantiate(projectile, tr);
            shot.GetComponent<ProjectileBehavior>().setup(projectileStrength, projectileSpeed, new Vector2(mousePos.x, mousePos.y), tr, gameObject.tag);
            timeShot = Time.time;
        }
    }

    public void attack()
    {
        if(Time.time - timeSwung > meleeCooldown)
        {
            hitBoxes[lastDir].enabled = true;
            swingDir = lastDir;
            slashAnimators[swingDir].SetInteger("TriggeredDir", swingDir);
            timeSwung = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //If we're eating a corpse or a plant
        if (collider.gameObject.tag == "Edible")
        {
            if(collider.gameObject.name == "blue-bird")
            {
                Debug.Log("Ate blue-bird (health)");
                health+=1;
            }
            if(collider.gameObject.name == "yellow-bird")
            {
                Debug.Log("Ate yellow-bird (speed)");
                speed+=1;
            }
            if(collider.gameObject.name == "brown-bird")
            {
                Debug.Log("Ate brown-bird (defense)");
                speed+=1;
            }
            if(collider.gameObject.name == "fire-lizard")
            {
                Debug.Log("Ate fire-lizard");
                fireStrength += 1;
            }
            if(collider.gameObject.name == "ice-lizard")
            {
                Debug.Log("Ate ice-lizard");
                iceStrength += 1;
            }
            if(collider.gameObject.name == "poison-bush")
            {
                Debug.Log("Ate poison-bush");
                poisonStrength += 1;
            }

            Destroy(collider.gameObject);
        }
        
        //Taking damage
        if(collider.IsTouching(hurtBox))
        {   
            if(collider.gameObject.tag == "Projectile" && collider.gameObject.GetComponent<ProjectileBehavior>().parTag != "Player")
            {
                Debug.Log("hit.");
                float str = collider.gameObject.GetComponent<ProjectileBehavior>().strength;
                Debug.Log(str);
                health -= Constants.calcDamage(str, defense);
            }

            if(collider.gameObject.tag == "Enemy")
            {
                float str = collider.gameObject.GetComponent<EnemyController>().strength;
                health -= Constants.calcDamage(str, defense);
            }
        }

        
    }

}
