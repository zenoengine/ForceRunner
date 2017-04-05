using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public static float ACCELERATION = 10.0f; // 가속도
    public static float SPEED_MIN = 4.0f; // 속도의 최솟값
    public static float SPEED_MAX = 8.0f; // 속도의 최댓값
    public static float JUMP_HEIGHT_MAX = 3.0f; // 점프 높이
    public static float JUMP_KEY_RELEASE_REDUCE = 0.5f; // 점프 후의 감속도
    public static float NARAKU_HEIGHT = -5.0f;

    private void check_landed() // 착지했는지 조사
    {
        this.is_landed = false; // 일단 false로 설정.
        do
        {
            Vector3 s = this.transform.position; // Player의 현재 위치.
            Vector3 e = s + Vector3.down * 1.0f; // s부터 아래로 1.0f로 이동한 위치.
            RaycastHit hit;
            if (!Physics.Linecast(s, e, out hit))
            { // s부터 e 사이에 아무것도 없을 때.
                break; // 아무것도 하지 않고 do~while 루프를 빠져나감(탈출구로).
            }
            // s부터 e 사이에 뭔가 있을 때 아래의 처리가 실행.
            if (this.step == STEP.JUMP)
            { // 현재, 점프 상태라면.
                if (this.step_timer < Time.deltaTime * 3.0f)
                { // 경과 시간이 3.0f 미만이라면.
                    break; // 아무것도 하지 않고 do~while 루프를 빠져나감(탈출구로).
                }
            }
            // s부터 e 사이에 뭔가 있고 JUMP 직후가 아닐 때만 아래가 실행.
            this.is_landed = true;
        } while (false);
        // 루프의 탈출구.
    }
    
    public enum STEP
    {
        NONE = -1, // 상태정보 없음
        RUN = 0, // 달린다
        JUMP, // 점프
        MISS, // 실패
        NUM, // 상태가
    };

    public STEP step = STEP.NONE; // Player
    public STEP next_step = STEP.NONE; // Player
    public float step_timer = 0.0f; // 경과 시간
    private bool is_landed = false; // 착지했는가
    private bool is_colided = false; // 뭔가와 충돌했는가
    private bool is_key_released = false; // 버튼이 떨어졌는가

    Rigidbody rb = null;
    // Use this for initialization
    void Start()
    {
        this.next_step = STEP.RUN;
        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = rb.velocity; // 속도를 설정
        check_landed(); // 착지 상태인지 체크
        this.step_timer += Time.deltaTime; // 경과 시간을 진행한다
                                           // 다음 상태가 정해져 있지 않으면 상태의 변화를 조사한다
        switch (this.step)
        {
            case STEP.RUN:
            case STEP.JUMP:
                // 현재 위치가 한계치보다 아래면.
                if (this.transform.position.y < NARAKU_HEIGHT)
                {
                    this.next_step = STEP.MISS; // '실패' 상태로 한다.
                }
                break;
            case STEP.MISS:
                // 가속도(ACCELERATION)를 빼서 Player의 속도를 느리게 해 간다.
                velocity.x -= PlayerControl.ACCELERATION * Time.deltaTime;
                if (velocity.x < 0.0f)
                { // Player의 속도가 마이너스면.
                    velocity.x = 0.0f; // 0으로 한다.
                }
                break;
        }

        if (this.next_step == STEP.NONE)
        {
            switch (this.step)
            { // Player
                case STEP.RUN: // 달리는 중일
                    if (!this.is_landed)
                    {
                        // 달리는 중이고 착지하지 않은 경우 아무것도 하지 않는다.
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            this.next_step = STEP.JUMP;
                        }
                    }
                    break;
                case STEP.JUMP: // 점프 중일
                    if (this.is_landed)
                    {
                        this.next_step = STEP.RUN;
                    }
                    break;
            }
        }
        // '다음 정보'가 '상태 정보 없음'이 아닌 동안(상태가 변할 때만).
        while (this.next_step != STEP.NONE)
        {
            this.step = this.next_step;// '현재 상태'를 '다음 상태'로 갱신.
            this.next_step = STEP.NONE;// '다음 상태'를 '상태 없음'으로 변경.
            switch (this.step)
            { // 갱신된 '현재 상태'가.
                case STEP.JUMP: // '점프'일 때.
                                // 최고 도달점 높이(JUMP_HEIGHT_MAX)까지 점프할 수 있는 속도를 계산. 
                    velocity.y = Mathf.Sqrt(2.0f * 9.8f * PlayerControl.JUMP_HEIGHT_MAX);
                    // '버튼이 떨어졌음을 나타내는 플래그'를 클리어한다.
                    this.is_key_released = false;
                    break;
            }
            // 상태가 변했으므로 경과 시간을 제로로 리셋.
            this.step_timer = 0.0f;
        }
        // 상태별로 매 프레임 갱신 처리.
        switch (this.step)
        {
            case STEP.RUN: // 달리는 중일 때.
                           // 속도를 높인다.
                velocity.x += PlayerControl.ACCELERATION * Time.deltaTime;
                // 속도가 최고 속도 제한을 넘으면.
                if (Mathf.Abs(velocity.x) > PlayerControl.SPEED_MAX)
                {
                    // 최고 속도 제한 이하로 유지한다.
                    velocity.x *= PlayerControl.SPEED_MAX / Mathf.Abs(rb.velocity.x);
                }
                break;
            // (계속)
            case STEP.JUMP: // 점프 중일
                do
                {
                    // '버튼이 떨어진 순간
                    if (!Input.GetMouseButtonUp(0))
                    {
                        break; // 아무것도 하지 않고 루프를 빠져나간다
                    }
                    // 이미 감속된 상태면

                    if (this.is_key_released)
                    {
                        break; // 아무것도 하지 않고 루프를 빠져나간다
                    }
                    // 상하방향 속도가 0 이하면
                    if (velocity.y <= 0.0f)
                    {
                        break; // 아무것도 하지 않고 루프를 빠져나간다
                    }
                    // 버튼이 떨어져 있고 상승 중이라면 감속 시작
                    // 점프의 상승은 여기서
                    velocity.y *= JUMP_KEY_RELEASE_REDUCE;
                    this.is_key_released = true;
                } while (false);
                break;
        }
        rb.velocity = velocity;
    }
}
