using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn
{
    public string attackerName; //name of attacker
    public bool isPlayer{
        get{
            return attacker.baseStats.isPlayer;
        }
    }
    public CombatantStateMachine attacker; //script of attacker
    public List<CombatantStateMachine> targets; //script being targeted by attack

    public CombatMove chosenAttack;

    public HandleTurn(string theAttacker=null, CombatantStateMachine attackerStats=null, List<CombatantStateMachine> targetStats=null, CombatMove theAttack=null){
        attackerName = theAttacker;
        attacker = attackerStats;
        targets = targetStats;
        chosenAttack = theAttack;
    }
}
