using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap_reposition : MonoBehaviour
{
    //Tilemap의 한변의 길이 저장
    private int Tile_length = 24;


    Collider2D  Collide;

    private void Awake() {
        
        Collide = GetComponent<Collider2D>();

    }

     
    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        //Area와 충돌이 아니라면, 그냥 retur해서 아래 코드 실행 안시키게
        if (!other.CompareTag("Area")) {
            return;
        }

        //Player와, 이 script를 넣은 대상의 position 가져오기
        Vector3 Player_pos = GameManager.instance.Player.transform.position;
        Vector3 My_pos = transform.position;

        //Player 좌표에서 My_pos를 빼, 플레이어의 이동 방향 검산하기?
        float X_direction = Player_pos.x - My_pos.x;
        float Y_direction = Player_pos.y - My_pos.y;

        //Player와 My_pos의 위치값 차이
        float X_difference = Mathf.Abs(X_direction);
        float Y_difference = Mathf.Abs(Y_direction);

        //삼향 연산자를 이용, 이동의 xy 방향을 +-1로 설정
        X_direction = X_direction > 0 ? 1 : -1;
        Y_direction = Y_direction > 0 ? 1 : -1;

         

        //이 함수의 tag에 따라 로직 변경
        switch (transform.tag) {
            
            //Map과의 Exit 상황 
            case "Map":

                //대각선 이동 시 Tilemap을 대각선으로 따라 이동해주어야 함
                //대각선 이동의 정의를, X와 Y의 차이가 거의 없을 때로 지정해줌
                if (Mathf.Abs(X_difference - Y_difference) <= 0.1f) {
                    
                    //대각선 이동 시, Tilemap을 이동방향에 맞게 상하좌우로 이동시켜주기
                    transform.Translate(Vector3.up * Y_direction * Tile_length * 2);
                    transform.Translate(Vector3.right * X_direction * Tile_length * 2);

                }

                //좌우 이동 시 Tilemap을 좌우로 따라 이동
                else if (X_difference > Y_difference) {

                    transform.Translate(Vector3.right * X_direction * Tile_length * 2);

                }

                //상하 이동 시 Tilemap을 상하로 따라 이동
                else if (X_difference < Y_difference) {

                    transform.Translate(Vector3.up * Y_direction * Tile_length * 2);

                }
                
                break;

            case "Enemy":

                if (Collide.enabled) {
                    transform.Translate(Vector3.right * X_direction * Tile_length);
                    transform.Translate(Vector3.up * Y_direction * Tile_length);
                }

                break;
        }
    }



     


}
