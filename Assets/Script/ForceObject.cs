using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObject : MonoBehaviour {
    GameObject player;
    ForceControl forceController;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        forceController = player.GetComponent<ForceControl>();
        forceController.AddObject(gameObject);
    }

    void OnDestroy()
    {
        forceController.RemoveObject(gameObject);
    }

    // Update is called once per frame
    void Update () {
        float sqrMagnitude = (gameObject.transform.position - player.transform.position).sqrMagnitude;
        if (sqrMagnitude > 300)
        {
            Destroy(gameObject);
        }
    }
}
