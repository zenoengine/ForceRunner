using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour {
    private PlayerControl player = null;
    public float step_timer = 0.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        this.step_timer += Time.deltaTime;

        if (this.player.isPlayEnd())
        {
            SceneManager.LoadScene("result");
        }
    }

    public float getPlayTime()
    {
        float time;
        time = this.step_timer;
        return (time);
    }

    public void ResetTime()
    {
        step_timer = 0;
    }
}
