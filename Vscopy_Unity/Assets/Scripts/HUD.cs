using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //열거형으로 선언해보기
    public enum Infotype {Exp, Level, Kills, Time, Hp}

    //열거형 변수 선언
    public Infotype Type;

    //Txt, Slider을 담을 변수 선언
    Text My_txt;
    Slider My_slider;




    private void Awake() {

        //Component 가져오기
        My_txt = GetComponent<Text>();
        My_slider = GetComponent<Slider>();
        
    }

    //열거형으로 선언한 데이터를 switch case 문을 이용해서 사용해보기
    //Inspertor에서 Type을 지정해주면, 그 Type에 따라 아래 switch문 작동
    private void LateUpdate() {
        

        switch (Type) {
            
            
            case Infotype.Exp:

                //현재, 최대 경험치 가져오기
                float Cur_exp = GameManager.instance.Exp;

                //무한 레벨업의 구현을 위해, 최대 경험치를 가져올 때 Mathf.Min 함수 사용
                //현 Level이 GameManager.instance.Next_exp.Length - 1 을 넘겨도, Min 함수 사용해서
                //indexoutofrange 방지
                float Max_exp = GameManager.instance.Next_exp[Mathf.Min(GameManager.instance.Level, GameManager.instance.Next_exp.Length - 1)];

                //Slider의 value를 경험치로 조정해주기
                My_slider.value = Cur_exp / Max_exp;


                break;

            case Infotype.Level:

                //Format문을 사용해 변수를 문자열에 넣기, F0을 통해 소수점 지워주기
                My_txt.text = string.Format("Lv.{0:F0}", GameManager.instance.Level) ;

                //아래는 SetText 함수를 이용한 버전, 그냥 Text가 아니라 TextMeshProGUI 형 변수다    
                //My_txt.SetText($"Lv.{GameManager.instance.Level}"); 


                break;

            case Infotype.Kills:

                //Level text와 똑같이 format문으로 int 변수를 문자열로 넣기
                My_txt.text = string.Format("{0:F0}", GameManager.instance.Kills) ;

                break;


            case Infotype.Time:

                //게임이 끝나는 시간 - 진행 시간, 남아있는 시간 저장
                float Remain_time = GameManager.instance.Max_game_time - GameManager.instance.Game_time;

                //남은 시간을 60으로 나눠서 분을 얻는데, FloorToInt를 통해 소수점을 버리자
                int Min = Mathf.FloorToInt(Remain_time / 60);

                //Python과 똑같이, % 연산자를 통해 나머지를 얻자
                int Sec = Mathf.FloorToInt(Remain_time % 60);

                //0:D2, 1:D2를 통해 최소한 2자리를 가지고 있게끔 함. 만약 Min이나 Sec이
                //한자리라면 자동으로 앞을 0으로 채워줌
                My_txt.text = string.Format("{0:D2}:{1:D2}", Min, Sec);

                break;


            case Infotype.Hp:

                //현재, 최대 체력 가져오기
                float P_Current_hp = GameManager.instance.P_Current_hp;
                float P_Max_hp = GameManager.instance.P_Max_hp;

                //Slider의 value를 현재 체력 비율로 조정
                My_slider.value = P_Current_hp / P_Max_hp;



                break;

                        




        }






    }





}
