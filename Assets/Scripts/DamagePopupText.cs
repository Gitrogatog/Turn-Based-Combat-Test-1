using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupText : MonoBehaviour
{
    public static DamagePopupText Create(Vector3 position, int damageAmount){
        DamagePopupText damagePopup = Instantiate(GameAssets.i.damagePopup, position, Quaternion.identity);
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }
    private static int sortingOrder;
    private const float DISAPPEAR_TIMER_MAX = 1f;

    public TextMeshPro tmp;
    private float disappearTimer;
    private float disappearSpeed;
    private Color textColor;
    private Vector3 moveVector;
    bool isActive = false;
    void Awake()
    {
        if(tmp == null){
            tmp = GetComponent<TextMeshPro>();
        }
    }

    // Update is called once per frame
    public void Setup(int damageAmount){
        if(tmp == null){
            tmp = GetComponent<TextMeshPro>();
        }
        tmp.SetText(damageAmount.ToString());
        textColor = tmp.color;
        isActive = true;
        disappearTimer = DISAPPEAR_TIMER_MAX;
        disappearSpeed = 3f;
        moveVector = new Vector3(0.7f, 1) * 30f;

        sortingOrder++;
        tmp.sortingOrder = sortingOrder;
    }

    void Update(){
        if(isActive){
            transform.position += moveVector * Time.deltaTime;
            moveVector -= moveVector * 8f * Time.deltaTime;

            if(disappearSpeed > DISAPPEAR_TIMER_MAX * 0.5){
                //First half of disappearance
                float increaseScaleAmount = 1f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else{
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            disappearTimer -= Time.deltaTime;
            if(disappearTimer <= 0){
                disappearSpeed = 3f;
                textColor.a = disappearSpeed * Time.deltaTime;
                tmp.color = textColor;
                if(textColor.a <= 0){
                    Destroy(gameObject);
                }
            }
        }
        
    }
}
