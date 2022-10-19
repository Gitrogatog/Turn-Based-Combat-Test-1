using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New CombatMove", menuName = "Combat Move", order = 51)]
public class CombatMove : ScriptableObject
{
    public string moveName;
    public string moveAnimName;
    public MoveType type;
    public bool areaOfEffect = false;
    public int cost;
    public int baseDamage;
    public bool isMelee;
    public List<MoveEffect> moveEffects;
    [System.Serializable]
    public class MoveEffect{
        public int damage = 0;
        public MoveEffectBase moveEffect;
    }

    public CombatMove(MoveType moveType){
        type = moveType;
    }
    
}
public enum MoveType{
    Attack, Defend, Heal, Flee, Pass
}
