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
        PUSHOUT,
        SUPER
    }
    PlayerControl playerControl = null;
    public ForceState state = ForceState.NONE;
    private List<ForceObject> forceObjects = new List<ForceObject>();
    private Dictionary<string, ForceState> stringStateMap = new Dictionary<string, ForceState>();
    private Dictionary<string, bool> tagSignMap = new Dictionary<string, bool>();
    GameRoot gameRoot = null;
    public ParticleSystem forceEffect = null;

    public delegate void OnChangeForceCount(int value);
    public OnChangeForceCount onChangeForceCountEvent;

    int forceCount = 0;
    static float MAX_SUPER_TIME = 10.0f;
    static int SUPER_FORCE_COUNT = 70;
    float SuperForceTimer = MAX_SUPER_TIME;

    void Start()
    {
        stringStateMap.Add("none", ForceState.NONE);
        stringStateMap.Add("attract", ForceState.ATTRACT);
        stringStateMap.Add("pushout", ForceState.PUSHOUT);

        tagSignMap.Add("coin", true);
        tagSignMap.Add("force", true);
        tagSignMap.Add("gumba", false);

        playerControl = GetComponent<PlayerControl>();
        gameRoot = GameObject.Find("GameRoot").GetComponent<GameRoot>();
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("force", (int)forceCount);
    }

    void Update()
    {
        if (forceCount >= SUPER_FORCE_COUNT)
        {
            AddForceItem(-SUPER_FORCE_COUNT);
            state = ForceState.SUPER;
            playerControl.OnStartSuperForce();
        }

        if (SuperForceTimer < 0)
        {
            state = ForceState.ATTRACT;
            SuperForceTimer = MAX_SUPER_TIME;
            playerControl.OnFinishSuperForce();
            forceEffect.Stop();
        }

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
            case ForceState.SUPER:
                {
                    sign = 1;
                    SuperForceTimer -= Time.deltaTime;
                    gameRoot.ResetTime();

                    if (!forceEffect.isPlaying)
                    {
                        forceEffect.Play();
                    }
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
            if (state == ForceState.SUPER)
            {
                if (obj.tag != null)
                {
                    bool value = false;
                    if (tagSignMap.TryGetValue(obj.tag, out value))
                    {
                        if (value)
                        {
                            sign = 1;
                        }
                        else
                        {
                            sign = -1;
                        }
                    }
                }
            }

            Vector3 dir = (gameObject.transform.position - obj.transform.position).normalized * sign;
            obj.Move(dir);
        }
    }

    public void AddObject(ForceObject obj)
    {
        forceObjects.Add(obj);
    }
    public void RemoveObject(ForceObject obj)
    {
        forceObjects.Remove(obj);
    }
    public void SetForceState(string newState)
    {
        ForceState value;
        if (!stringStateMap.TryGetValue(newState, out value))
        {
            Debug.Log("Warning: SetForceState Function's parameter is wrong string.");
            return;
        }
        state = value;
    }

    public void AddForceItem(int value)
    {
        forceCount += value;
        onChangeForceCountEvent((int)forceCount);
    }
}
