using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatMenuManagerScript : MonoBehaviour
{
    public static CombatMenuManagerScript instance;
    public CombatMovesMenuScript battleMenu;
    //public string[] actionNames = {"Attack", "Magic", "Pass", "Flee"};
    public CombatantStateMachine currentPlayer;
    private HandleTurn currentTurn;
    private List<CombatantStateMachine> players;
    private List<CombatantStateMachine> enemies;
    private int currentTargetIndex = 0;
    private GameObject targetUI; 
    private List<GameObject> multiTargetEnemyUI;
    private List<GameObject> multiTargetPlayerUI;
    private float nextInputTime = 0;

    public CombatActionButtonScript actionButtonPrefab;
    public EnemySelectButton enemyButtonPrefab;
    public MenuOptionScript menuButtonPrefab;
    public GameObject buttonPanel;
    public enum SelectState{
        NotSelecting, Action, Magic, Target, Submit, Perform
    }
    public SelectState currentState  { get; private set;}
    //public GeneralMenuScript ActionMenu;
    //public GeneralMenuScript MagicMenu;
    //public GeneralMenuScript TargetMenu;

    //public static CombatMenuManagerScript instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        currentState = SelectState.NotSelecting;
        enemies = TurnCombatManagerScript.instance.enemies;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPlayerTurn(CombatantStateMachine selectedPlayer){
        currentTurn = new HandleTurn(selectedPlayer.name, selectedPlayer);
        //ActionMenu.gameObject.SetActive(true);
        currentPlayer = selectedPlayer;
        FillActionMenu();
    }

    void FillActionMenu(){
        battleMenu.ClearMenu();
        //List<UnityAction> actions = new List<UnityAction>();
        //List<string> actionNames = new List<string>();
        List<MenuOptionScript> actionMenuButtons = new List<MenuOptionScript>();
        //attack button
        CombatActionButtonScript attackButton = Instantiate(actionButtonPrefab, transform.position, transform.rotation);
        attackButton.chosenMove = currentPlayer.baseStats.basicAttack;
        attackButton.textBox.text = "Attack";
        actionMenuButtons.Add(attackButton);
        CombatActionButtonScript defendButton = Instantiate(actionButtonPrefab, transform.position, transform.rotation);
        defendButton.chosenMove = ScriptableObject.CreateInstance<CombatMove>();//new CombatMove(MoveType.Defend);
        defendButton.chosenMove.type = MoveType.Defend;
        defendButton.textBox.text = "Defend";
        actionMenuButtons.Add(defendButton);
        CombatActionButtonScript fleeButton = Instantiate(actionButtonPrefab, transform.position, transform.rotation);
        fleeButton.chosenMove = ScriptableObject.CreateInstance<CombatMove>();
        fleeButton.chosenMove.type = MoveType.Flee;
        fleeButton.textBox.text = "Flee";
        actionMenuButtons.Add(fleeButton);
        if(currentPlayer.baseStats.moves.Count > 0){
            MenuOptionScript magicButton = Instantiate(menuButtonPrefab, transform.position, transform.rotation);
            magicButton.OnPressed.AddListener(() => OpenMagicMenu());
            magicButton.textBox.text = "Magic";
            actionMenuButtons.Add(magicButton);
        }
        //battleMenu.menuOptions = actionMenuButtons;
        AddButtonsToMenu(actionMenuButtons);
        //ActionMenu.ClearMenu();
        //ActionMenu.FillMenu();

    }

    void FillMagicMenu(){
        battleMenu.ClearMenu();
        List<MenuOptionScript> magicMenuButtons = new List<MenuOptionScript>();
        foreach(CombatMove move in currentPlayer.baseStats.moves){
            CombatActionButtonScript attackButton = Instantiate(actionButtonPrefab, transform.position, transform.rotation);
            attackButton.chosenMove = move;
            attackButton.textBox.text = move.moveName;
            magicMenuButtons.Add(attackButton);
        }
        AddButtonsToMenu(magicMenuButtons);
    }

    public void OpenMagicMenu(){
        battleMenu.ClearMenu();
        FillMagicMenu();
        //ActionMenu.gameObject.SetActive(false);
        //MagicMenu.gameObject.SetActive(true);
    }

    public void BeginTargetSelect(){
        battleMenu.ClearMenu();
        List<MenuOptionScript> enemyMenuButtons = new List<MenuOptionScript>();
        foreach(CombatantStateMachine enemy in enemies){
            EnemySelectButton enemyButton = Instantiate(enemyButtonPrefab, transform.position, transform.rotation);
            enemyButton.SetEnemy(enemy);
            enemyButton.textBox.text = enemy.baseStats.unitName;
            enemyMenuButtons.Add(enemyButton);
        }
        AddButtonsToMenu(enemyMenuButtons);
    }

    public void ReceivePlayerTarget(CombatantStateMachine target){
        currentTurn.targets = new List<CombatantStateMachine>();
        currentTurn.targets.Add(target);
        SendTurnToTurnManager();
    }
    public void ReceivePlayerTargets(List<CombatantStateMachine> targets){
        currentTurn.targets = targets;
        SendTurnToTurnManager();
    }

    public void ReceivePlayerMove(CombatMove selectedMove){
        currentTurn.chosenAttack = selectedMove;
        if(selectedMove.type == MoveType.Attack || selectedMove.type == MoveType.Heal){
            BeginTargetSelect();
        }
        else{
            SendTurnToTurnManager();
        }
    }

    void SendTurnToTurnManager(){
        battleMenu.ClearMenu();
        TurnCombatManagerScript.instance.ReceiveTurn(currentTurn);
    }

    void AddButtonToMenu(MenuOptionScript newButton){
        newButton.transform.SetParent(buttonPanel.transform, false);
        battleMenu.menuOptions.Add(newButton);
    }

    void AddButtonsToMenu(List<MenuOptionScript> newButtons){
        foreach(MenuOptionScript newButton in newButtons){
            newButton.transform.SetParent(buttonPanel.transform, false);
        }
        battleMenu.AddButtonsToMenu(newButtons);
    }
}
