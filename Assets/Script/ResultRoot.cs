using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultRoot : MonoBehaviour {

    public Text scoreText;
    public Text forceText;
    public Text highScoreText;
    
    void Start () {
        SoundManager.Instance.PlayMusic("bgm2", false);
        int score = PlayerPrefs.GetInt("score",0);
        int force = PlayerPrefs.GetInt("force",0);
        int highScore = PlayerPrefs.GetInt("highscore", 0);

        if(score> highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
        }

        scoreText.text = score.ToString();
        forceText.text = force.ToString();
        highScoreText.text = highScore.ToString();
    }

    public void GoTitle()
    {
        SceneManager.LoadScene("title");
    }
}
