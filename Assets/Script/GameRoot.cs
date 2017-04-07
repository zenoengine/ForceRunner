using UnityEngine;
using System.Collections;

public class GameRoot : MonoBehaviour {

    public float step_timer = 0.0f; // 경과 시간을 유지한다.
    void Update()
    {
        this.step_timer += Time.deltaTime; // 경과 시간을 더해 간다.
    }
    public float getPlayTime()
    {
        float time;
        time = this.step_timer;
        return (time); // 호출한 곳에 경과 시간을 알려준다.
    }

    public void ResetTime()
    {
        step_timer = 0;
    }
    
}
