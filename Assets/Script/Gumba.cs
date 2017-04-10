using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gumba : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SoundManager.Instance.PlaySound("r_se_cat", false);
	}
	

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerControl>();
            collision.gameObject.SendMessage("OnCollsionEnterDeathObject");
        }
    }
}
