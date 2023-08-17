using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_control : MonoBehaviour
{
    //이동속도를 담을 변수
    public float Enemy_speed;

    //현재 체력, 최대 체력을 담을 변수
    public float Current_hp;

    public float Max_hp;

    //Animator을 받을 변수
    public RuntimeAnimatorController[] Run_ani_con; 

    //Player의 Rigidbody 2d를 받을 변수
    public Rigidbody2D Target_rb2;

    //Unit의 생존, 사망을 확인할 Bool 변수
    bool Live_dead;

    //Rigidbody 2d, Spriterenderer을 받을 변수
    Rigidbody2D Rb2;
    SpriteRenderer Sprite;

    Animator Anim;

    //Collider2d를 받을 변수, 사망 후에도 collider 충돌 처리 발생을 막기 위해
    Collider2D Coll_2d;

    //일정시간만큼 기다리게 하는 Waitfor 형을 담을 변수
    WaitForSeconds Wait;

    private void Awake() {

        //Rigidbody 2d, Spriterenderer 가져오기
        Rb2 = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        Coll_2d = GetComponent<Collider2D>();

        //매 Coroutine 마다 new를 생성하는 대신, 변수로 제어하기
        Wait = new WaitForSeconds(0f);
        
    }


    //이동을 Fixedupdate로 구현
    private void FixedUpdate() {


        //GameManager의 Pause(게임의 일시정지 유무)를 확인하고, Pause시 Update문 못들어가게 return
        if (GameManager.instance.Pause) {
            return;
        }


        //죽었을 시, return 시켜서 아래 이동 코드를 안따라가게 함
        //Animotor의, GetCurrentAnimatorStateInfo(index)를 통해, 이 unit의
        //Animator의 특정 index번째의 State를 알 수 있다. 
        //IsName을 통해 State의 이름이 특정 문자열과 같은지 다른지 확인 가능
        //피격 시, AddForce를 해주지만 지속적으로 velocity를 조정해주기에 티가 안남
        //이러한 문제를 해결하기 위해, 피격시 Animator의 State가 변경되면 이동을 중지시킴
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") ||!Live_dead){
           // print("Hit");
            return;

        }
        
        //Target에서 자신의 위치를 빼, Target을 향한 벡터 구성
        Vector2 Direction_vec = Target_rb2.position - Rb2.position;
        Vector2 N_vec = Direction_vec.normalized * Enemy_speed;

        Rb2.velocity = N_vec;

    }

    private void LateUpdate() {

        //GameManager의 Pause(게임의 일시정지 유무)를 확인하고, Pause시 Update문 못들어가게 return
        if (GameManager.instance.Pause) {
            return;
        }

        
        //Target의 x좌표가 자신의 x좌표보다 작으면, flipx를 통해 세로선 기준 뒤집기
        Sprite.flipX = Target_rb2.position.x < Rb2.position.x;

    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {   
        //Prefab은 활성화되지 않은 상태이기에, Scene의 Unit에 접근할 수 없다.
        //OnEnable로 Unit이 Enable되었을 때, GamaManager에 접근해 Player의 정보 가져오기
        Target_rb2 = GameManager.instance.Player.GetComponent<Rigidbody2D>();

        //Object Pooling으로 Unit을 계속 재활용하며 사용할 것이기에, OnEnable시
        //Live를 true로 해준다.
        Live_dead = true;

        //위와 같은 이유로, 사망 후 Disable 되었으면 체력이 0 이하일 것이기에
        //체력을 최대체력으로 설정해준다
        Current_hp = Max_hp;
    
        //Object pooling으로 Enemy를 관리하기에, 사망 시 각종 component를 비활성화한 것을
        //다시 활성화 시켜주어야 한다
        Coll_2d.enabled = true;  
        Rb2.simulated = true;   

         
        Sprite.sortingOrder += 1;     
        Anim.SetBool("Dead", false);

    }

    //Enemy의 data를 가져오는 함수를 생성, Spawn_data class를 통째로 가져오기
    public void Get_data(Spawn_data data) {

        Anim.runtimeAnimatorController = Run_ani_con[data.Sprite_type];

        Enemy_speed = data.E_speed;

        Current_hp = data.Hp;
        Max_hp = data.Hp;


    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {

        

        
        //Enter한 물체의 Tag가 Bullet이 아니면, 혹은 죽어있는 상태면 return
        if (!other.CompareTag("Bullet") || !Live_dead) {
            return;
        }   

        //Currnet_hp 에서, 충돌한 Bullet의 dmg만큼 빼주기
        Current_hp -= other.GetComponent<Bullet_control>().Bullet_dmg;

        //피격 시 coroutine 부르기
        StartCoroutine("Knock_back");

        //피격 후 생존 시
        if (Current_hp > 0) {

            //Animator의 trigger을, Hit으로 변경해주기
            Anim.SetTrigger("Hit");
             

        }


        //체력이 0 이하가 되 사망시
        else if (Current_hp <= 0) {

            //생존을 확인하는 Bool 변경
            Live_dead = false;
       
            Coll_2d.enabled = false; //Collider2d를 enable = false를 통해 중지
            Rb2.simulated = false; //Rigidbody 2d의 simulated = false를 통해 물리연산 중지

            //Order in Layer 값을 낮춰주어, 다른 Unit을 가리는 것을 방지
            Sprite.sortingOrder -= 1;  
            
            //Animator의 특정 Bool형 변수를 조작, Animation 변경
            Anim.SetBool("Dead", true);

            //Dead(); //Invoke 대신, Animator에서도 함수 실행 가능


            GameManager.instance.Kills++;
            GameManager.instance.Get_exp();



        }


    }


    //피격 시 넉백을 구현한 Coroutine
    IEnumerator Knock_back() {

        //print("Knock");
        yield return  Wait; // 다음 1프레임까지 딜레이

        //피격 시, 플레이어의 반대 방향으로 AddForce해서 넉백 구현
        Vector3 Player_pos = GameManager.instance.Player.transform.position;
        Vector3 Dir_vec = transform.position - Player_pos;

        //Impulse는 뭐지
        Rb2.AddForce(Dir_vec.normalized * 5, ForceMode2D.Impulse);

         


    }
    



    //사망시 처리 함수
    void Dead() {
        
        //Object pulling으로 관리하기에, Destroy 대신 SetActive false, 비활성화시키기
        gameObject.SetActive(false);

    }



}
