using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Renderer renderer;
    public Vector2 aim;
    public Transform tr;
    public Rigidbody2D rb;

    public float strength;
    public float speed;

    public bool destroySoon;

    public string parTag;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destroySoon = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!renderer.isVisible)
            Destroy(gameObject);
        if(destroySoon)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision) 
    {
        // if its not hitting itself and not hitting the creator
        if(collision.gameObject.tag != "Projectile" && collision.gameObject.tag != parTag)
        {
            destroySoon = true;
        }
            
    }

    public void setup(float strn, float spd, Vector2 aim, Transform ogPos, string parTag)
    {
        tr = ogPos;
        this.aim = aim;
        strength = strn;
        speed = spd;
        this.parTag = parTag;
        rb = GetComponent<Rigidbody2D>();
        launch();
    }

    void launch()
    {
        Vector2 trajectory = new Vector2(aim.x - tr.position.x, aim.y - tr.position.y);
            
        float magnitude = Mathf.Sqrt(trajectory.sqrMagnitude);

        if(magnitude > 0)
        {
            trajectory = new Vector2(trajectory.x * speed / magnitude, trajectory.y  * speed/ magnitude);
        }

        rb.velocity = trajectory;
    }
}
