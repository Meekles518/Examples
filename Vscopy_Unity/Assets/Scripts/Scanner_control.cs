using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner_control : MonoBehaviour
{
    //자동 원거리 공격 구현

    //자동 공격의 적 탐색 범위
    public float Scan_range;

    //CircleCastAll 함수에서, 탐색할 Layer을 저장할 변수
    public LayerMask Target_layer;

    public RaycastHit2D[] Targets;


    //가장 가까운 적의 위치 변수
    public Transform Nearest_target;




    private void FixedUpdate() {
        
        //원의 범위로 주변을 Cast하는 함수(스캔 위치, 원의 반지름, 스캔 방향, 스캔 거리, Layermask)
        //주변에서 Scan도중, 목표 대상이 잡히면 Targets List에 넣기?
        Targets = Physics2D.CircleCastAll(transform.position, Scan_range, Vector2.zero, 0, Target_layer);

        //가장 가까운 Target을 저장하기
        Nearest_target = Nearest();




    }

    //가장 가까운 적을 return해주는 함수
    Transform Nearest() {

        //가장 가까운 Target을 돌려줄 변수
        Transform Result = null;

        //Player와 가장 가까운 Target의 거리를 저장할 변수, 초기값은 임의로 100으로 설정
        float Difference = 100f;

        //Targets에 들어있는 모든 원소에 foreach로 접근
        foreach (RaycastHit2D Target in Targets) {

            //Player의 좌표와, Target의 좌표를 가져오기
            Vector3 My_pos = transform.position;
            Vector3 Target_pos = Target.transform.position;

            //Distance를 통해 Player와 Targer의 거리를 가져오기
            float Current_difference = Vector3.Distance(My_pos, Target_pos);

            //현재 foreach문의 Target과의 거리가, 저장되어 있는 Player - Target 간의 거리보다 짧으면
            //Difference와 Result를 새로 초기화
            if (Current_difference < Difference) {

                //더 가까운 거리, Target으로 교체
                Difference = Current_difference;
                Result = Target.transform;
            }
        }

        return Result;
    }
}
