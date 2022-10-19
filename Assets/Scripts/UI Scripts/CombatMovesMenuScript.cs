using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovesMenuScript : MonoBehaviour
{
    public List<MenuOptionScript> menuOptions;
    //public float inputSpeed = 0.3f;
    //public GameObject menuOptionPrefab;

    private float lastInputTime;
    private int currentIndex;
    private GameInputs currentInputs;
    bool freezeConfirm = false;
    float nextUnfreezeConfirmTime;
    void Start()
    {
        PlayerInputManagerScript.instance.OnUpdateInputs += UpdateInputs;
        //menuOptions = new List<MenuOptionScript>();
        lastInputTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastInputTime > TurnCombatGlobals.inputSpeed && menuOptions.Count > 0){
            //Debug.Log(menuOptions.Count);
            float vertical = currentInputs.move.y;
            int oldIndex = currentIndex;
            if(vertical > 0.1f){
                currentIndex--;
            }
            else if(vertical < -0.1f){
                currentIndex++;
            }
            if(currentIndex < 0){
                currentIndex = menuOptions.Count - 1;
            }
            else if(currentIndex >= menuOptions.Count){
                currentIndex = 0;
            }
            if(oldIndex != currentIndex){
                ChangeIndex(menuOptions[oldIndex], menuOptions[currentIndex]);
            }
            
            lastInputTime = Time.time;
            //Debug.Log(menuOptions[currentIndex].textBox.text);
            //Debug.Log("Vertical: " + vertical);
            //Debug.Log("Current Index: " + currentIndex);
            
        }
        if(currentInputs.confirm && nextUnfreezeConfirmTime < Time.time){
            nextUnfreezeConfirmTime = Time.time + TurnCombatGlobals.inputSpeed;
            ChooseMove();
        }
    }

    public void ChangeIndex(MenuOptionScript old, MenuOptionScript current){
        if(old != current){
            old.Unselect();
            current.Select();
        }
    }

    public void ChooseMove(){
        Debug.Log(currentIndex);
        Debug.Log(menuOptions[currentIndex].gameObject.name);
        menuOptions[currentIndex].Pressed();
        //ClearMenu();
    }

    public void UpdateInputs(GameInputs newInputs){
        currentInputs = newInputs;
    }

    public void ClearMenu(){
        Debug.Log("CLEAR");
        foreach(MenuOptionScript menuOption in menuOptions){
            Destroy(menuOption.gameObject);
        }
        menuOptions.Clear();
    }

    public void AddButtonsToMenu(List<MenuOptionScript> newMenuOptions){
        menuOptions = newMenuOptions;
        currentIndex = 0;
        Debug.Log("Buttons Added!");
        Debug.Log(menuOptions.Count);
    }
}
