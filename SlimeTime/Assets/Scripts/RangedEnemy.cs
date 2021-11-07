using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyController
{
    public float range;

    void FixedUpdate()
    {
        prepareUpdate();
        moveProperly();
        attackProperly();
    }

    public void moveProperly()
    {
        Vector3 enemyPos = targetPos.position;
        float distToTarget = Mathf.Sqrt(Mathf.Pow(enemyPos.x - rb.position.x, 2) + Mathf.Pow(enemyPos.y - rb.position.y, 2));

        if(distToTarget > range)
        {
            move(trackTarget());
        }
        else
        {
            move(new Vector2(0, 0));
        }
    }

    public void attackProperly()
    {
        shoot();
    }
}
