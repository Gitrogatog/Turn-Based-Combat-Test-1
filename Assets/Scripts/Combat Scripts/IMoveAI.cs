using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAI
{
    public HandleTurn SelectMove(List<CombatantStateMachine> players, List<CombatantStateMachine> enemies);
}
