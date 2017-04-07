using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    Text scoreText;
    GameManager gameManager;
    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.onChangeScoreEvent += ChangeValue;
        scoreText = GetComponent<Text>();
    }

    void ChangeValue(float value)
    {
        scoreText.text = "Score : " + ((int)value).ToString();
    }
}
