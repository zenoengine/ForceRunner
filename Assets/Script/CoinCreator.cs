using UnityEngine;
using System.Collections;

public class CoinCreator : MonoBehaviour {
    public GameObject[] coinPrefabs; // 블록을 저장할 배열
    private int coin_count = 0; // 생성한 블록의 개수

    public MapCreator map_creator = null; // MapCreator를 보관하는 변수
    void Start()
    {
        // MapCreator를 가져와서 멤버 변수 map_creator에 보관
        map_creator = GameObject.Find("GameRoot")
        .GetComponent<MapCreator>();
    }

    public void createBlock(Vector3 block_position)
    {
        int next_item_type = coin_count % coinPrefabs.Length; 
        GameObject go = GameObject.Instantiate(coinPrefabs[next_item_type]) as GameObject;
        go.transform.position = block_position; 
        this.coin_count++; 
    }
}
