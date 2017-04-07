using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObject : MonoBehaviour {
    GameObject player;
    ForceControl forceController;

    public float ForceSpeed = 1.0f;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        forceController = player.GetComponent<ForceControl>();
        forceController.AddObject(this);
    }

    void OnDestroy()
    {
        forceController.RemoveObject(this);
    }

    public void Move(Vector3 translateVector)
    {
        gameObject.transform.Translate(translateVector * ForceSpeed * Time.deltaTime);
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
