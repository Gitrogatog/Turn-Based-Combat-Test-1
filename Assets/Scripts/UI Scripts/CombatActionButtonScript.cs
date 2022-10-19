using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionButtonScript : MenuOptionScript
{
    public CombatMove chosenMove;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //Debug.Log(textBox.text + " " + chosenMove.type);
        OnPressed.AddListener(() => SendMoveToCombatMenu());
        if(chosenMove.cost < CombatMenuManagerScript.instance.currentPlayer.baseStats.currentMana){
            pressable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SendMoveToCombatMenu(){
        Debug.Log(textBox.text + " " + chosenMove.type);
        CombatMenuManagerScript.instance.ReceivePlayerMove(chosenMove);
    }
}
