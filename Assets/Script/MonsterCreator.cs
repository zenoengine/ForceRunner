using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCreator : MonoBehaviour {
    public GameObject[] monster_prefab; 
    private int monster_count = 0;

    public MapCreator map_creator = null; // MapCreator를 보관하는 변수
    void Start()
    {
        // MapCreator를 가져와서 멤버 변수 map_creator에 보관
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
    }

    public void createMonster(Vector3 monster_position)
    {
        int next_monster_type = monster_count % monster_prefab.Length;
        GameObject go = GameObject.Instantiate(monster_prefab[next_monster_type]) as GameObject;
        go.transform.position = monster_position;
        this.monster_count++;
    }
}
