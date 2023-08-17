using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_control : MonoBehaviour
{ 
    //Bullet의 dmg를 저장할 변수
    public float Bullet_dmg;

    //Bullet의 Per, 관통 정도를 담당할 변수(근접무기는 항상 관통함)
    public int Per;

    //원거리 무기의 이동 속도를 조정할 변수
    public float Range_weapon_speed = 10f;

    //원거리 무기의 Rigidbody 2d를 위한 변수
    Rigidbody2D Rb2;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Rb2 = GetComponent<Rigidbody2D>();
    }


    //Bullet의 dmg, Per을 초기화시켜주는 함수
    public void Init(float Bullet_dmg, int Per , Vector3 Direction) {

        //this를 통해 Init함수 외부의 변수에 접근
        this.Bullet_dmg = Bullet_dmg;

        this.Per = Per;

        //근접 무기가 아닌 경우(근접 무기는 기본 Per값이 -1임)
        if (Per > -1) {

            //원거리 무기의 경우, 총알이 이동을 해야 하니 받은 벡터, Direction으로 velocity 해주기
            Rb2.velocity = Direction * Range_weapon_speed;

        }



    }



    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        //근접 무기이거나, 적과 부딪힌게 아니다 라면 그냥 Return
        if(!other.CompareTag("Enemy") || Per == -1) {
            return;
        }

        //적과 충돌 후 관통 수치 -1 해주기
        Per -= 1;

        //관통 수치가 0일 때 적과 충돌해 수치가 음수가 되면, 총알을 비활성화 시키기
        //비활성화 시키는 이유는, Pooling으로 총알을 관리하기 때문
        if (Per < 0) {
            //Pooling을 위해, 혹시 모르니 velocity값을 0벡터로 초기화
            Rb2.velocity = Vector2.zero;

            //Object pooling을 통환 재활용을 위해 비화성화 시키기
            gameObject.SetActive(false);

        }


    }













}
