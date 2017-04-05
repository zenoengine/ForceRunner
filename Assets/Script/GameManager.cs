using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public enum LEVEL
    {
        LEVEL_MAIN = 0,
        LEVEL_GAME,
        LEVEL_RESULT
    }

    public float score = 0;
    public int top_score = 0;
    public int collected_force = 0;

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
