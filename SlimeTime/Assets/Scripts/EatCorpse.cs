using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatCorpse : MonoBehaviour
{
    public float Increase;
    public float Decrease;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Suh dude");
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.tag == "Food")
        {
            transform.localScale += new Vector3(Increase, Increase, Increase);
            Destroy(other.gameObject);
        }
    }

    private int debounce = 31;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && debounce > 30)
        {
            debounce = 0;
            Debug.Log("Shoot projectiles i guess");
            
        }
        else {
            debounce++;
        }

    }
}
