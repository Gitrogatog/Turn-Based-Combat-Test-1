using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_HadoukenEffect : MoveEffectBase
{
   public float moveSpeed = 5f;
   bool reachedTarget = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(effectTargets.Count > 0){
            transform.position = Vector2.MoveTowards(transform.position, effectTargets[0].transform.position, moveSpeed * Time.deltaTime);
            if(Vector2.SqrMagnitude(transform.position - effectTargets[0].transform.position) < 1f && !reachedTarget){
                DealDamage();
                EndEffect();
                reachedTarget = true;
                Destroy(gameObject);
            }
        }
    }
}
