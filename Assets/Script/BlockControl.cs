using UnityEngine;
using System.Collections;

public class BlockControl : MonoBehaviour {
    // BlockControl.cs
    public MapCreator map_creator = null; // MapCreator를 보관하는 변수
    void Start()
    {
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
    }
    void Update()
    {
        if (this.map_creator.isDelete(this.gameObject))
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
