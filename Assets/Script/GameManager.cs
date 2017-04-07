using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    int preScore = 0;
    public float score = 0;
    public delegate void OnChangeScore(float score);
    public OnChangeScore onChangeScoreEvent;

    void Start()
    {
    }

    private void Update()
    {
        score += Time.deltaTime;

        if(preScore < (int)score)
        {
            preScore = (int)score;
            onChangeScoreEvent(score);
        }
    }

    public void AddScore(int value)
    {
        score += value;
        onChangeScoreEvent(score);
    }

}
