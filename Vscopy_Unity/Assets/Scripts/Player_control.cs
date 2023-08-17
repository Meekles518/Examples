using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_control : MonoBehaviour
{
    //Scanner_control Script를 받을 변수
    public Scanner_control Scanner_Control;

    //Player의 양 손을 관리하기 위해 사용할 변수
    public Hand_control[] Hands;

    //선택한 Character의 Animator 받기 위한 배열
    public RuntimeAnimatorController[] Animcon;


    //키보드 입력의 벡터를 받을 변수
    public Vector2 Input_vec;

    //Rigidbody 2d component, Spriterenderer를 받을 변수
    public Rigidbody2D Rb2;
    private SpriteRenderer Sprite;

    //Animator 받을 변수
    private Animator Ani;

    //이동속도 변수
    public float Player_speed;




    private void Awake() {
        
        Rb2 = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        Ani = GetComponent<Animator>();
        Scanner_Control = GetComponent<Scanner_control>();

        //true를 인자로 주면, Disable 된 object도 접근해서 component 가져옴
        Hands = GetComponentsInChildren<Hand_control>(true);

    }

    //Player 활성화 시 작동
    private void OnEnable() {

        //속성을 이용해 특정 값을 return받기
        Player_speed *= Char_Adv.Speed;

        //Player의 Animator을 Animcon 배열에 있는 특정 idx의 것으로 바꾸기
        Ani.runtimeAnimatorController = Animcon[GameManager.instance.Player_idx];
    }




    // Update is called once per frame
    void Update()
    {
        //GameManager의 Pause(게임의 일시정지 유무)를 확인하고, Pause시 Update문 못들어가게 return
        if (GameManager.instance.Pause) {
            return;
        }


        //좌우, 상하 Input값 받아서 벡터에 넣기
        Input_vec.x = Input.GetAxisRaw("Horizontal");
        Input_vec.y = Input.GetAxisRaw("Vertical");

        
    }

    //고정된 시간마다 작동하는, FixedUpdate로 이동을 구현
    private void FixedUpdate() {
        

        Rb2.velocity = Input_vec.normalized * Player_speed;

    }

    //매 프레임 종료 후에 작동되는 LateUpdate
    private void LateUpdate() {

        //GameManager의 Pause(게임의 일시정지 유무)를 확인하고, Pause시 Update문 못들어가게 return
        if (GameManager.instance.Pause) {
            return;
        }
        
        //SetFloat로 벡터의 magnitued, 벡터의 길이 전달해주기
        Ani.SetFloat("Speed", Input_vec.magnitude);


        //이동중이고, 좌측 이동이라면
        if (Input_vec.x != 0 ) {

            //flipx를 True로 해주어, 캐릭터를 뒤집어준다
            Sprite.flipX = Input_vec.x < 0;

        }

    }


    //Player의 피격 판정
    private void OnCollisionStay2D(Collision2D other) {
        
        //Pause 상태일 시 return
        if (GameManager.instance.Pause) {

            return;

        }

        //임의로 피격 시 데미지 10 구성
        GameManager.instance.P_Current_hp -= Time.deltaTime * 10;

        //Player의 Child로 있는 Spawner, left right hand를 비활성화
        if (GameManager.instance.P_Current_hp <= 0) {

            for (int idx = 1; idx < transform.childCount; idx++) {

                transform.GetChild(idx).gameObject.SetActive(false);

            }

            //Animator의 trigger 설정으로 애니메이션 바꾸기
            Ani.SetTrigger("Dead");

            //GameOver 함수 실행
            GameManager.instance.GameOver();



        }




    }







}
