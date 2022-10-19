using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public Vector2 statRange;
    public List<float> multipliers;

    public float Evaluate(){
        float result = Random.Range(statRange.x, statRange.y);
        foreach(float mult in multipliers){
            result *= mult;
        }
        return result;
    }

    public int EvaluateInt(){
        float result = Random.Range(statRange.x, statRange.y);
        foreach(float mult in multipliers){
            result *= mult;
        }
        return Mathf.RoundToInt(result);
    }
}
