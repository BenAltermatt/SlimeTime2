using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public const float MAX_DEF = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // normalized equation for damage dealing
    public static float calcDamage(float strength, float def)
    {
        return strength * (MAX_DEF - def) / MAX_DEF;
    }
}
