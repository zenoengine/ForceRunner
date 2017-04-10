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
    public static float FLY_HEIGHT = 5.0f;

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
                if (this.step_timer < Time.deltaTime * 2.5f)
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
        NONE = -1,
        RUN = 0,
        JUMP,
        MISS,
        FLY,
        NUM,
    };

    public STEP step = STEP.NONE; // Player
    public STEP next_step = STEP.NONE; // Player
    public float step_timer = 0.0f; // 경과 시간
    private bool is_landed = false; // 착지했는가
    //private bool is_colided = false; // 뭔가와 충돌했는가
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
                    SoundManager.Instance.PlaySound("r_jingle_fall", false);
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
            case STEP.FLY:
                {
                    velocity.y = 0;
                    velocity.z = 0;
                    velocity.x = 10;
                    Vector3 flyPosition = this.transform.position;
                    if (flyPosition.y <= FLY_HEIGHT)
                    {
                        flyPosition.y += Time.deltaTime * 5;
                    }

                    this.transform.position = flyPosition;
                }
                break;
        }

        if (this.next_step == STEP.NONE)
        {
            switch (this.step)
            {
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
        }
        rb.velocity = velocity;
    }

    public void Jump()
    {
        switch (this.step)
        {
            case STEP.RUN: // 달리는 중일
                if (!this.is_landed)
                {
                }
                else
                {
                    this.next_step = STEP.JUMP;
                    SoundManager.Instance.PlaySound("r_se_jump", false);
                }
                break;
        }
    }

    public void OnStartSuperForce()
    {
        rb.useGravity = false;
        next_step = STEP.FLY;
    }

    public void OnFinishSuperForce()
    {
        rb.useGravity = true;
        step = STEP.RUN;
    }

    public bool isPlayEnd() // 게임이 끝났는지 판정.
    {
        bool ret = false;
        switch (this.step)
        {
            case STEP.MISS: // MISS 상태라면.
                ret = true; // '죽었어요'(true)라고 알려줌.
                break;
        }
        return (ret);
    }


    public void OnCollsionEnterDeathObject()
    {
        this.step = STEP.MISS;
    }
}
