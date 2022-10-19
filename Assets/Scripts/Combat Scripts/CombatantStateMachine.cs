using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatantStateMachine : MonoBehaviour
{
    public CombatantStats baseStats;
    public bool isControlledByPlayer;
    public IMoveAI aIBase;
    public enum CombatantState{
        Idle, Selecting, PerformAction, MoveToward, MoveBack, Dead
    }

    public CombatantState currentState = CombatantState.Idle;
    private Vector3 startPos;
    private Vector3 targetPos;
    public float moveSpeed = 5f;
    Animator animator;
    public HandleTurn currentTurn;
    bool startedAction = false;
    int activeMoveEffects = 0;
    bool attackAnimActive = false;
    private string currentAnimState = "Idle";

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public Transform healthBar;
    public RectTransform rectHealthBar;
    public Transform manaBar;
    public RectTransform rectManaBar;

    void Start()
    {
        startPos = transform.position;
        if(!isControlledByPlayer){
            aIBase = GetComponent<IMoveAI>();
        }
        animator = GetComponent<Animator>();
        UpdateHealthBar();
        UpdateManaBar();
    }

    void FixedUpdate()
    {
        switch(currentState){
            case(CombatantState.Idle):
            break;
            case(CombatantState.Selecting):
            break;
            case(CombatantState.PerformAction):
                if(!startedAction){
                    startedAction = true;
                    //StartCoroutine(PlayMove(currentTurn));
                    PlayMove(currentTurn);
                }
            break;
            case(CombatantState.MoveToward):
                ChangeAnimState("Moving");
                if(MoveTowardsTarget(targetPos)){
                    currentState = CombatantState.PerformAction;
                    //animator.SetBool("Moving", false);
                }
            break;
            case(CombatantState.MoveBack):
                ChangeAnimState("Moving");
                if(MoveTowardsTarget(targetPos)){
                    EndAttackPhase();
                    //animator.SetBool("Moving", false);
                }
            break;
            case(CombatantState.Dead):
            break;
        }
    }

    void PlayMove(HandleTurn chosenTurn){
        switch(chosenTurn.chosenAttack.type){
            case(MoveType.Attack):
                /*
                if(chosenTurn.chosenAttack.isMelee){
                    ChangeAnimState("Melee Attack");
                }
                else{
                    ChangeAnimState("Ranged Attack");
                }
                */
                ChangeAnimState(chosenTurn.chosenAttack.moveAnimName);
                attackAnimActive = true;
                LoseMana(chosenTurn.chosenAttack.cost);
            break;
            case(MoveType.Defend):
                Debug.Log("Defending");
                AttemptEndAttackPhase();
            break;
            case(MoveType.Flee):
                Debug.Log("Fleeing");
                AttemptEndAttackPhase();
            break;
        }
        
    }
    /*
    private IEnumerator PlayMove(HandleTurn chosenTurn){
        if(chosenTurn.chosenAttack.type == MoveType.Attack){
            Debug.Log("Initiate Attack");
            while(chosenTurn.chosenAttack.isMelee && !MoveTowardsTarget(targetPos)){
                animator.SetBool("Moving", true);
                yield return null;
            }
            animator.SetBool("Moving", false);
            if(chosenTurn.chosenAttack.isMelee){
                animator.SetTrigger("Melee Attack");
            }
            else{
                animator.SetTrigger("Ranged Attack");
            }
            
            yield return new WaitForSeconds(0.5f);
            //DealDamage();
            Debug.Log("Damage Dealt");
            yield return new WaitForSeconds(0.5f);
            if(chosenTurn.chosenAttack.isMelee){
                animator.ResetTrigger("Melee Attack");
            }
            else{
                animator.ResetTrigger("Ranged Attack");
            }
            while(chosenTurn.chosenAttack.isMelee && !MoveTowardsTarget(startPos)){
                animator.SetBool("Moving", true);
                yield return null;
            }
            animator.SetBool("Moving", false);
            animator.SetTrigger("Idle");
        }
        else if(chosenTurn.chosenAttack.type == MoveType.Defend){
            Debug.Log("Defending");
            yield return new WaitForSeconds(0.1f);
        }
        else if(chosenTurn.chosenAttack.type == MoveType.Flee){
            Debug.Log("Fleeing");
            yield return new WaitForSeconds(0.1f);
        }
        EndAttackPhase();
    }
    */

    private bool MoveTowardsTarget(Vector3 target){
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime));
    }

    public void SetCombatantTurn(HandleTurn newTurn){
        currentTurn = newTurn;
        if(newTurn.chosenAttack.type == MoveType.Attack && newTurn.chosenAttack.isMelee){
            currentState = CombatantState.MoveToward;
            ChangeAnimState("Moving");
        }
        else{
            currentState = CombatantState.PerformAction;
        }
        
    }

    public void DealDamage(){
        foreach(CombatantStateMachine targetStateMachine in currentTurn.targets){
            targetStateMachine.TakeDamage(currentTurn.chosenAttack.baseDamage);
        }
    }
    /*
    public void MoveEffectDealDamage(int effectDamage, CombatantStateMachine effectTarget){
        effectTarget.TakeDamage(effectDamage);
    }

    public void MoveEffectDealDamage(int effectDamage, List<CombatantStateMachine> effectTargets){
        foreach(CombatantStateMachine targetStateMachine in effectTargets){
            targetStateMachine.TakeDamage(effectDamage);
        }
    }
    */

    public void TakeDamage(int damage){
        baseStats.currentHealth -= damage;
        if(baseStats.currentHealth <= 0){
            baseStats.currentHealth = 0;
            currentState = CombatantState.Dead;
        }
        UpdateHealthBar();
    }

    public void LoseMana(int manaLoss){
        baseStats.SpendMana(manaLoss);
        UpdateManaBar();
    }

    public void CreateMoveEffect(int moveEffectIndex){
        if(currentTurn.chosenAttack.moveEffects.Count > 0){
            moveEffectIndex = Mathf.Clamp(moveEffectIndex, 0, currentTurn.chosenAttack.moveEffects.Count - 1);
            MoveEffectBase moveEffect = Instantiate(currentTurn.chosenAttack.moveEffects[moveEffectIndex].moveEffect, transform.position, transform.rotation);
            moveEffect.PlayMoveEffect(this, currentTurn.targets, currentTurn.chosenAttack.moveEffects[moveEffectIndex].damage);
            activeMoveEffects++;
        }
        
    }

    public void ReceiveAttackAnimEnd(){
        if(attackAnimActive){
            attackAnimActive = false;
            if(activeMoveEffects <= 0){
                AttemptEndAttackPhase();
            }
        }
    }

    public void ReceiveMoveEffectEnd(){
        activeMoveEffects--;
        if(activeMoveEffects <= 0 && !attackAnimActive){
            AttemptEndAttackPhase();
        }
    }

    void AttemptEndAttackPhase(){
        currentState = CombatantState.MoveBack;
        ChangeAnimState("Moving");
    }

    void EndAttackPhase(){
        activeMoveEffects = 0;
        currentState = CombatantState.Idle;
        ChangeAnimState("Idle");
        startedAction = false;
        currentTurn = null;
        //Tell TurnCombatManager that this unit has finished their attack
        TurnCombatManagerScript.instance.ReceiveTurnEnd();
    }

    void ChangeAnimState(string newState){
        if(currentAnimState != newState){
            animator.Play(newState);
            currentAnimState = newState;
        }
    }

    void UpdateHealthBar(){
        if(healthText != null){
            healthText.text = baseStats.currentHealth + "/" + baseStats.maxHealth;
        }
        if(healthBar != null){
            healthBar.localScale = new Vector3((float)baseStats.currentHealth/baseStats.maxHealth, healthBar.localScale.y);
        }
        if(rectHealthBar != null){
            rectHealthBar.localScale = new Vector3((float)baseStats.currentHealth/baseStats.maxHealth, rectHealthBar.localScale.y);
        }
    }

    void UpdateManaBar(){
        if(manaText != null){
            manaText.text = baseStats.CurrentMana + "/" + baseStats.maxMana;
        }
        if(manaBar != null){
            manaBar.localScale = new Vector3((float)baseStats.CurrentMana/baseStats.maxMana, manaBar.localScale.y);
        }
        if(rectManaBar != null){
            rectManaBar.localScale = new Vector3((float)baseStats.CurrentMana/baseStats.maxMana, rectManaBar.localScale.y);
        }
    }
}
