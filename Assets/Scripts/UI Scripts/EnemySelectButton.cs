using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MenuOptionScript
{
    public CombatantStateMachine enemy;
    private bool showSelector;
    private GameObject selector;

    protected override void Start()
    {
        base.Start();
        if(enemy != null){
            selector = enemy.gameObject.transform.Find("Selector").gameObject;
        }
        OnSelected.AddListener(() => ShowSelector());
        OnUnselected.AddListener(() => HideSelector());
        OnPressed.AddListener(() => SelectEnemy());
    }

    public void SetEnemy(CombatantStateMachine newEnemy){
        enemy = newEnemy;
        selector = enemy.gameObject.transform.Find("Selector").gameObject;
    }

    public void SelectEnemy(){
        Debug.Log("Selection Made");
        CombatMenuManagerScript.instance.ReceivePlayerTarget(enemy);
        HideSelector();
    }

    public void ShowSelector(){
        if(selector != null){
            selector.SetActive(true);
        }
    }

    public void HideSelector(){
        if(selector != null){
            selector.SetActive(false);
        }
    }
}
