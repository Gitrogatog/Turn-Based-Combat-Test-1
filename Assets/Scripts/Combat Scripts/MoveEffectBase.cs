using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffectBase : MonoBehaviour
{
    protected CombatantStateMachine originalUser;
    protected List<CombatantStateMachine> effectTargets;
    int effectDamage;
    /*
    public float initWaitTime;
    public float playTime;
    public float endTime;
    float updateStateTime;
    enum MoveEffectState{
        StartWait, Play, EndWait
    }
    MoveEffectState effectState = MoveEffectState.StartWait;
    */
    public void PlayMoveEffect(CombatantStateMachine newUser, List<CombatantStateMachine> newTargets, int damage){
        originalUser = newUser;
        effectTargets = newTargets;
        effectDamage = damage;
        //updateStateTime = Time.time + initWaitTime;
    }

    protected void DealDamage(){
        foreach(CombatantStateMachine targetStateMachine in effectTargets){
            targetStateMachine.TakeDamage(effectDamage);
        }
    }

    protected void DealDamage(CombatantStateMachine specificTarget){
        specificTarget.TakeDamage(effectDamage);
    }

    protected void EndEffect(){
        originalUser.ReceiveMoveEffectEnd();
    }
    /*
    void Update(){
        switch(effectState){
            case MoveEffectState.StartWait :
                if(updateStateTime < Time.time){
                    effectState = MoveEffectState.Play;
                    updateStateTime = Time.time + playTime;
                }
                break;
            case MoveEffectState.Play :
                if(updateStateTime < Time.time){
                    effectState = MoveEffectState.EndWait;
                    updateStateTime = Time.time + endTime;
                }
                break;
            case MoveEffectState.EndWait :
                if(updateStateTime < Time.time){
                    Destroy(gameObject);
                }
                break;
        }
    }
    */
}
