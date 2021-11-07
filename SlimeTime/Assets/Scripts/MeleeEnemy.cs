using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyController
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
        move(trackTarget());
    }

    public void attackProperly()
    {
        Vector3 enemyPos = targetPos.position;
        float distToTarget = Mathf.Sqrt(Mathf.Pow(enemyPos.x - rb.position.x, 2) + Mathf.Pow(enemyPos.y - rb.position.y, 2));

        if(distToTarget <= range)
        {
            attack();
        }
    }
}
