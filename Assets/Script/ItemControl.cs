using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour {
    private MapCreator map_creator = null;
    private GameObject player = null;
    void Start()
    {
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
        player = GameObject.Find("Player");
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
            DestroyObject(this.gameObject);
        }
    }   
}
