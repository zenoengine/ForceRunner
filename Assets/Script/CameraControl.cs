using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    // CameraControl.cs
    private GameObject player = null;
    private Vector3 position_offset = Vector3.zero;

    void Start()
    {
        // 멤버 변수 player에 Player 오브젝트를 할당
        this.player = GameObject.FindGameObjectWithTag("Player");
        // 카메라 위치(this.transform.position)와
        // 플레이어 위치(this.player.transform.position)의 차이를 보관
        this.position_offset = this.transform.position - this.player.transform.position;
    }

    void LateUpdate()
    {
        // 카메라 현재 위치를 new_position에 할당
        Vector3 new_position = this.transform.position;
        // 플레이어의 X좌표에 차이 값을 더해서 new_position의 X에 대입
        new_position.x = this.player.transform.position.x + this.position_offset.x;
        // 카메라 위치를 새로운 위치(new_position)로 갱신
        this.transform.position = new_position;
    }
}
