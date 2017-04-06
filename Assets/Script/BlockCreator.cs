using UnityEngine;
using System.Collections;

public class BlockCreator : MonoBehaviour {
    // BlockCreator.cs
    public GameObject[] blockPrefabs; // 블록을 저장할 배열
    public GameObject[] spineBlocksPrefabs;
    private int block_count = 0; // 생성한 블록의 개수

    public MapCreator map_creator = null; // MapCreator를 보관하는 변수
    void Start()
    {
        // MapCreator를 가져와서 멤버 변수 map_creator에 보관
        map_creator = GameObject.Find("GameRoot")
        .GetComponent<MapCreator>();
    }
    
    public void CreateBlock(Vector3 block_position)
    {
        // 만들어야 할 블록의 종류(흰색인가 빨간색인가)를 구한다
        int next_block_type = this.block_count % this.blockPrefabs.Length; // % : 나머지를 구하는 연산자
                                                                           // 블록을 생성하고 go에 보관한다
        GameObject go = GameObject.Instantiate(this.blockPrefabs[next_block_type]) as GameObject;
        go.transform.position = block_position; // 블록의 위치를 이동
        this.block_count++; // 블록의 개수를 증가
    }

    public void CreateSpineBlock(Vector3 block_position)
    {
        // 만들어야 할 블록의 종류(흰색인가 빨간색인가)를 구한다
        int next_block_type = this.block_count % this.spineBlocksPrefabs.Length; // % : 나머지를 구하는 연산자
                                                                           // 블록을 생성하고 go에 보관한다
        GameObject go = GameObject.Instantiate(this.spineBlocksPrefabs[next_block_type]) as GameObject;
        go.transform.position = block_position; // 블록의 위치를 이동
        this.block_count++; // 블록의 개수를 증가
    }
}
