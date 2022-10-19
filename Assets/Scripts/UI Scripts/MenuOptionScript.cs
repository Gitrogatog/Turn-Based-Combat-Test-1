using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class MenuOptionScript : MonoBehaviour
{
    public UnityEvent OnSelected;
    public UnityEvent OnUnselected;
    public UnityEvent OnPressed;
    public TextMeshProUGUI textBox;
    public bool pressable = true;

    public Animator anim;

    protected virtual void Start(){
        //anim = GetComponent<Animator>();
        if(textBox == null){
            textBox = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void Unselect(){
        //anim.SetTrigger("Unselected");
        //anim.ResetTrigger("Selected");
        anim.Play("Selected");
        OnUnselected?.Invoke();
    }

    public void Select(){
        //anim.SetTrigger("Selected");
        //anim.ResetTrigger("Unselected");
        anim.Play("Unselected");
        OnSelected?.Invoke();
    }

    public void Pressed(){
        //anim.SetTrigger("Pressed");
        //anim.ResetTrigger("Unselected");
        if(pressable){
            anim.Play("Pressed");
            OnPressed?.Invoke();
        }
        
    }
}
