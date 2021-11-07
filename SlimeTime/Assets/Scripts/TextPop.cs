using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPop : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 60;

    public GameObject gameObject;
    private float disappearTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject = 
        disappearTimer = DISAPPEAR_TIMER_MAX;     
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Hi");
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            Destroy(gameObject);
        }   
    }
}
