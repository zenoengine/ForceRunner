using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatableItem : MonoBehaviour {
    private MapCreator map_creator = null;
    private GameObject player = null;
    private ForceControl forceControl = null;
    private GameManager gameManager = null;
    void Start()
    {
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        forceControl = player.GetComponent<ForceControl>();
    }
    void Update()
    {
        if (this.map_creator.is_delete(this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }

        float distance = (gameObject.transform.position - player.transform.position).magnitude;

         
        if (distance < 1.5f)
        {
            if(this.tag.Contains("coin"))
            {
                gameManager.AddScore(10);
            }
            else if(this.tag.Contains("force"))
            {
                forceControl.AddForceItem(2);
            }

            DestroyObject(this.gameObject);
        }
    }   
}
