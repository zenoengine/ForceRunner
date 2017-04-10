using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleRoot : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlayMusic("bgm2",false);
	}
	
    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }
}
