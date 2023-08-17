using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand_control : MonoBehaviour
{
    //좌우 확인을 위한 bool 변수
    public bool Is_left;

    //Player와 Hand의 Sprite를 저장할 변수
    public SpriteRenderer Sprite;

    //Player의 flipx 여부를 알기 위해 사용할 변수
    SpriteRenderer Player;

    //왼손과 오른손 좌표 미리 기입
    Vector3 Righthand_pos = new Vector3(0.35f, 0.1f, 0);
    Vector3 Reverse_Righthand_pos = new Vector3(-0.35f, 0.1f, 0);

    Quaternion Lefthand_q = Quaternion.Euler(0, 0, -35);
    Quaternion Reversed_Lefthand_q = Quaternion.Euler(0, 0, 35);

    private void Awake() {
        
        //GetComponentsInParent는 자기 자신이 첫번째로 들어가니, 0대신 1로 idx 접근
        Player = GetComponentsInParent<SpriteRenderer>()[1];

    }

    //무기의 위치 변환 및 회전 처리
    private void LateUpdate() {
        
        bool Flip = Player.flipX;

        //왼손, 근접 무기일 경우
        if (Is_left) {


            //Player의 flipx 가 True인지 False인지에 따라 무기의 flip 변환
            transform.localRotation = Flip ? Reversed_Lefthand_q : Lefthand_q;
            Sprite.flipX = Flip;
            Sprite.sortingOrder = Flip ? 4 : 6;


        }

        //오른손, 원거리 무기일 경우
        else {

            transform.localPosition = Flip ? Reverse_Righthand_pos : Righthand_pos;
            Sprite.flipX = Flip;
            Sprite.sortingOrder = Flip ? 6 : 4;

        }
    }



}
