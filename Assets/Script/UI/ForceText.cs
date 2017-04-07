using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceText : MonoBehaviour {
    ForceControl forceControl = null;
    public Text forceText;
	// Use this for initialization
	void Start () {
        forceControl = GameObject.Find("Player").GetComponent<ForceControl>();
        forceControl.onChangeForceCountEvent += new ForceControl.OnChangeForceCount(ChangeValue);
        forceText = GetComponent<Text>();
    }

    void ChangeValue(int value)
    {
        forceText.text = "Force : " + value.ToString();
    }
}
