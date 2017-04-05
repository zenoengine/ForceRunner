using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemCreator : MonoBehaviour {
    [Serializable]
    public struct ItemPrefab
    {
        public ItemType type;
        public GameObject prefab;
    }

    public ItemPrefab[] itemPrefabs;

    private MapCreator map_creator = null;

    void Start()
    {
        // MapCreator를 가져와서 멤버 변수 map_creator에 보관
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
    }

    public void createItem(ItemType type, Vector3 item_position)
    {
        GameObject prefab = null;
        if(FindPrefabFromType(type, out prefab))
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.position = item_position;
        }
    }

    bool FindPrefabFromType(ItemType type, out GameObject prefab)
    {
        prefab = null;
        foreach (var item in itemPrefabs)
        {
            if (item.type == type)
            {
                prefab = item.prefab;
                return true;
            }
        }

        return false;
    }
}
