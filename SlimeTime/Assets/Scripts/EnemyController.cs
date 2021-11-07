using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float health;
    public float defense; 
    public float strength;
    public float shootCooldown;
    public float swingSpeed;
    public float projectileStrength;
    public float projectileSpeed;

    private float timeShotted;

    // melee stuff
    public float meleeCooldown;
    private float timeSwung;
    public int swingDir;

    private bool toBeDestroyed;

    // hitboxes and hurtboxes
    public Collider2D hurtBox;
    public Collider2D[] hitBoxes;

    public const int RIGHT = 0;
    public const int DOWN = 1;
    public const int LEFT = 2;
    public const int UP = 3;

    private int lastDir;
    
    public Rigidbody2D rb;
    public GameObject target;
    public GameObject projectile;
    public Transform tr;
    private Transform targetPos;

    // animator managers
    public Animator moveAnimator;
    public Animator[] slashAnimators;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = target.GetComponent<Transform>();
        tr = GetComponent<Transform>();
        timeShotted = Time.time;
        timeSwung = Time.time;

        lastDir = DOWN;
        swingDir = DOWN;
        health = 100;
        toBeDestroyed = false;

        Collider2D[] boxes = GetComponents<Collider2D>();
        hitBoxes = new Collider2D[4];
        hurtBox = boxes[5];
        for(int i = 1; i < boxes.Length - 1; i++)
        {
            hitBoxes[i - 1] = boxes[i];
            hitBoxes[i - 1].enabled = false;
        }

        Animator[] allAnimators = GetComponentsInChildren<Animator>();
        slashAnimators = new Animator[4];
        moveAnimator = allAnimators[0];
        for(int i = 1; i < allAnimators.Length; i++)
        {
            slashAnimators[i - 1] = allAnimators[i];
        }

        isMoving = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    void runAway() {
        Vector3 moveDir = tr.position - targetPos.position;
        moveDir = new Vector3(moveDir.x, moveDir.y, 0);
        transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }

    void FixedUpdate() {
        if(toBeDestroyed)
        {
            Destroy(gameObject);
        }
        if(health <= 0)
        {
            toBeDestroyed = true;
        }
        move(trackTarget());
        if(Input.GetMouseButton(0))
            shoot();
        if(Time.time - timeSwung > swingSpeed)
        {
            hitBoxes[swingDir].enabled = false;
        }
        if(Input.GetKey("space"))
        {
            attack();
        }
    }

    // collects all collisions
    void OnTriggerEnter2D(Collider2D collider) {
        // detect if hit by a projectile
        if(collider.IsTouching(hurtBox))
        {
            if(collider.gameObject.tag == "Projectile" && collider.gameObject.GetComponent<ProjectileBehavior>().parTag != "Enemy")
            {
                float projStrength = collider.gameObject.GetComponent<ProjectileBehavior>().strength;
                health -= Constants.calcDamage(projStrength, defense);
            }

            // detect if hit by melee attack
            if(collider.gameObject.tag == "Player")
            {
                float pStren = collider.gameObject.GetComponent<PlayerController>().strength;
                health -= Constants.calcDamage(pStren, defense);
            }
        }
    }

    // moves at a velocity and updates animations and stuff as necessary
    void move(Vector2 velocity)
    {
        // figure out direction
        if(Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x)) // vertical will override
        {
            if(velocity.y > 0)
                lastDir = 3;
            else if(velocity.y < 0)
                lastDir = 1;
        }
        else
        {
            if(velocity.x > 0)
                lastDir = 0;
            else
                lastDir = 2;
        }
        
        // are we moving?
        if(velocity.sqrMagnitude > 0)
        {
            isMoving = true;
        }

        // update the animation accoridngly
        moveAnimator.SetInteger("Direction", lastDir);
        moveAnimator.SetBool("Moving", isMoving);
        rb.velocity = velocity;
    }

    // properly follows the target
    private Vector2 trackTarget()
    {
        Vector3 targPos = targetPos.position;
        Vector2 trajectory = new Vector2(targPos.x- rb.position.x, targPos.y - rb.position.y);
        float magnitude = trajectory.sqrMagnitude;

        if(magnitude != 0)
        {
            trajectory = trajectory * speed / magnitude;
        }

        return trajectory;

    }

    public void attack()
    {
        if(Time.time - timeSwung > meleeCooldown)
        {
            hitBoxes[lastDir].enabled = true;
            swingDir = lastDir;
            timeSwung = Time.time;
            slashAnimators[lastDir].SetTrigger("Trigger");
            moveAnimator.SetTrigger("Attack");
        }
    }

    // shoot at the target
    void shoot()
    {
        if(Time.time - timeShotted > shootCooldown) // it's cooled down
        {
            Vector3 targPos = targetPos.position;

            GameObject boolet = GameObject.Instantiate(projectile, tr);
            boolet.GetComponent<ProjectileBehavior>().setup(projectileStrength, projectileSpeed, 
            new Vector2(targPos.x, targPos.y), tr, gameObject.tag);

            timeShotted = Time.time;
        }
    }
}
