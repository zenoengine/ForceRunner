using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ForceControl : MonoBehaviour
{
    [Serializable]
    public enum ForceState
    {
        NONE = 0,
        ATTRACT,
        PUSHOUT
    }


    public ForceState state = ForceState.NONE;
    private List<GameObject> forceObjects = new List<GameObject>();
    private Dictionary<string, ForceState> stringStateMap = new Dictionary<string, ForceState>();

    public void AddObject(GameObject obj)
    {
        forceObjects.Add(obj);
    }
    public void RemoveObject(GameObject obj)
    {
        forceObjects.Remove(obj);
    }
    public void SetForceState(string newState)
    {
        ForceState value; 
        if(!stringStateMap.TryGetValue(newState, out value))
        {
            Debug.Log("Warning: SetForceState Function's parameter is wrong string.");
            return;
        }
        state = value;
    }

    float power = 10;

    void Start()
    {
        stringStateMap.Add("none", ForceState.NONE);
        stringStateMap.Add("attract", ForceState.ATTRACT);
        stringStateMap.Add("pushout", ForceState.PUSHOUT);
    }

    void Update()
    {
        int sign = 1;
        switch (state)
        {
            case ForceState.ATTRACT:
                {
                    sign = 1;
                }
                break;
            case ForceState.PUSHOUT:
                {
                    sign = -1;
                }
                break;
            default:
                {
                    sign = 0;
                }
                break;
        }

        foreach (var obj in forceObjects)
        {
            Vector3 dir = (gameObject.transform.position - obj.transform.position).normalized * sign;
            obj.transform.Translate(dir * Time.deltaTime * power);
        }
    }
}
