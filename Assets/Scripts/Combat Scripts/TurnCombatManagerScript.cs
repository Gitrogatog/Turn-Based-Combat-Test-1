using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TurnCombatManagerScript : MonoBehaviour
{
    public static TurnCombatManagerScript instance;
    public List<Transform> playerTransforms;
    public List<Transform> enemyTransforms;
    public List<CombatantStateMachine> players;
    public List<CombatantStateMachine> enemies;

    public CombatantStats[] playersTest, enemiesTest;

    private List<CombatantStateMachine> moveOrder;
    private HandleTurn currentTurnSetup;
    private List<CombatantStateMachine> targets;

    public CombatMenuManagerScript battleMenuManager;
    //private GameObject targetUI;
    //private List<GameObject> multipleTargetUI;

    //public PlayerMoveSelectorScript playerMoveSelector;
    private CombatantStateMachine actingUnit;
    private int moveOrderIndex = 0;
    public GameObject playerPanel;
    public PlayerBarScript playerBarPrefab;

    public enum CombatState{
        Start, NextTurn, PlayerSelect, EnemySelect, Perform, CheckAlive, Win, Lose, Cutscene
    }

    public CombatState currentState = CombatState.Start;
    //private EventSystem eventSystem;

    public bool PlayersAlive{
        get{
            foreach(CombatantStateMachine player in players){
                if(player.baseStats.isAlive){
                    return true;
                }
            }
            return false;
        }
        
    }

    public bool EnemiesAlive{
        get{
            foreach(CombatantStateMachine enemy in enemies){
                if(enemy.baseStats.isAlive){
                    return true;
                }
            }
            return false;
        }
        
    }
    public bool InCombat{
        get{return PlayersAlive && EnemiesAlive;}
    }
    public event Action OnCombatStart;
    public event Action<CombatResult> OnCombatEnd;

    [ContextMenu("Start Combat")]
    private void TEST_Combat(){
        //StartTurnCombat(playersTest, enemiesTest);
        StartTurnCombat();
    }

    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }

    private void Start(){
        targets = new List<CombatantStateMachine>();
        //multipleTargetUI = new List<GameObject>();
        //targetUI = Instantiate(Resources.Load("Battle Target") as GameObject);
        //targetUI.SetActive(false);
        //battleMenuManager = Instantiate((Resources.Load("Battle Menu Manager") as GameObject)).GetComponent<CombatMenuManagerScript>();
        //battleMenuManager.GetComponent<Canvas>().worldCamera = Camera.main;
        //battleMenuManager = FindObjectOfType<CombatMenuManagerScript>();
        //eventSystem = FindObjectOfType<EventSystem>();
        StartTurnCombat();
    }

    private void Update(){
        switch(currentState){
            case(CombatState.NextTurn):
                Debug.Log("NEXT TURN");
                moveOrderIndex ++;
                if(moveOrderIndex >= moveOrder.Count){
                    Debug.Log("Loop Over Turns!");
                    moveOrderIndex = 0;
                }
                actingUnit = moveOrder[moveOrderIndex];
                if(actingUnit.baseStats.isPlayer){
                    StartPlayerTurn(actingUnit);
                    currentState = CombatState.PlayerSelect;
                }
                else{
                    //start enemy move select
                    currentState = CombatState.EnemySelect;
                    StartEnemyTurn(actingUnit);
                }
            break;
            case(CombatState.PlayerSelect):
            break;
            case(CombatState.Perform):
            break;
        }
    }

    public void StartTurnCombat(){
        moveOrder = new List<CombatantStateMachine>();
        foreach(CombatantStateMachine playerSM in players){
            moveOrder.Add(playerSM);
            PlayerBarScript newPlayerBar = Instantiate(playerBarPrefab, transform.position, Quaternion.identity);
            playerSM.rectHealthBar = newPlayerBar.healthBar;
            playerSM.rectManaBar = newPlayerBar.manaBar;
            playerSM.healthText = newPlayerBar.healthText;
            playerSM.manaText = newPlayerBar.manaText;
        }
        foreach(CombatantStateMachine enemySM in enemies){
            moveOrder.Add(enemySM);
        }
        moveOrderIndex = 0;
        currentState = CombatState.PlayerSelect;
        actingUnit = moveOrder[moveOrderIndex];
        if(actingUnit.baseStats.isPlayer){
            StartPlayerTurn(actingUnit);
        }
        else{
            StartEnemyTurn(actingUnit);
        }
    }

    void StartPlayerTurn(CombatantStateMachine actingPlayer){
        battleMenuManager.StartPlayerTurn(actingPlayer);
        Debug.Log("Stuff");
    }

    void StartEnemyTurn(CombatantStateMachine actingEnemy){
        ReceiveTurnEnd();
    }

    public void ReceiveTurn(HandleTurn newTurn){
        currentTurnSetup = newTurn;
        currentState = CombatState.Perform;
        actingUnit.SetCombatantTurn(currentTurnSetup);
    }

    public void ReceiveTurnEnd(){
        currentState = CombatState.NextTurn;
    }
}

public enum CombatResult{
    Ran, Won, Lost
}
