using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatantStats : MonoBehaviour
{
    public string unitName;
    public bool isPlayer;
    [Header("Stats")]
    public int maxHealth;
    public int maxMana;
    public int attackSpeed;
    public int damage;
    public int agility;
    public int defense;

    [Space(10)]
    [Header("Moves")]
    public CombatMove basicAttack;
    public List<CombatMove> moves;
 
    [Space(10)]
    [Header("Events")]
    public UnityEvent OnHurt, OnDeath;

    public int currentHealth;
    private Animator animator;
    public int CurrentHealth{
        get{return currentHealth;}
    }
    public int currentMana;
    public int CurrentMana{
        get{return currentMana;}
    }
    public bool isAlive{
        get{return currentHealth > 0;}
    }
    void Start()
    {
        //currentHealth = maxHealth.EvaluateInt();
        currentHealth = maxHealth;
        currentMana = maxMana;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage){
        currentHealth -= Mathf.RoundToInt(damage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if(currentHealth == 0){
            if(OnDeath != null){
                OnDeath.Invoke();
            }
        }
        else{
            if(OnHurt != null){
                OnHurt.Invoke();
            }
        }
    }

    public void SpendMana(int cost){
        currentMana -= cost;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    [ContextMenu("Evaluate")]
    public void TEST_Evaluate(){
        //Debug.Log(maxHealth.Evaluate());
    }
}
