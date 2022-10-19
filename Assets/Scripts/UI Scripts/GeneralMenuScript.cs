using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralMenuScript : MonoBehaviour
{
    public MenuOptionScript[] menuOptionPrefabs;
    public MenuOptionScript baseMenuOptionPrefab;
    public List<MenuOptionScript> menuOptions;
    public GameObject actionsPanel;
    bool isActive = false;
    int selectedMenuOption = 0;
    public CombatMovesMenuScript battleMenu; //associated battle menu
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearMenu(){
        foreach(MenuOptionScript menuOption in menuOptions){
            Destroy(menuOption.gameObject);
        }
        menuOptions.Clear();
        battleMenu.menuOptions.Clear();
    }

    public void FillMenu(){
        foreach(MenuOptionScript menuOption in menuOptionPrefabs){
            MenuOptionScript createdMenuOption = Instantiate(menuOption, transform.position, transform.rotation);
            createdMenuOption.transform.SetParent(actionsPanel.transform, false);
            menuOptions.Add(createdMenuOption);
        }
    }

    void FillMenu(List<MenuOptionScript> newMenuOptions){
        foreach(MenuOptionScript menuOption in newMenuOptions){
            MenuOptionScript createdMenuOption = Instantiate(menuOption, transform.position, transform.rotation);
            createdMenuOption.transform.SetParent(actionsPanel.transform, false);
            menuOptions.Add(createdMenuOption);
            //createdMenuOption.OnPressed += 
        }
    }

    void FillMenu(List<UnityAction> buttonActions){
        ClearMenu();
        foreach(UnityAction buttonAction in buttonActions){
            MenuOptionScript createdMenuOption = Instantiate(baseMenuOptionPrefab, transform.position, transform.rotation);
            createdMenuOption.transform.SetParent(actionsPanel.transform, false);
            createdMenuOption.OnPressed.AddListener(buttonAction);
            menuOptions.Add(createdMenuOption);
            battleMenu.menuOptions.Add(createdMenuOption);
        }
    }

    void SaveSelectedMenuOption(){
        
    }
}
