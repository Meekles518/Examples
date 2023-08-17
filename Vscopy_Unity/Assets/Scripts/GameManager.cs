using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //싱글톤 패턴 사용? 자기 자신을 변수에 저장해, 다른곳에서 호출하게 함
    //메모리에 적재한다고 표현한다?
    public static GameManager instance;

    [Header("Single tone")]
    //GameManager에 다른 주요한 Unit?을 넣어서 GameManager에 접근하는 것으로
    //다른 것들에게도 접근 가능하게 함
    public PoolManager Pool;
    public Player_control Player;

    public LevelUpUI_control LevelUpUI;

    [Header("Player info")]
    //레벨, 킬 수, 현재 경험치, 각 레벨의 필요 경험치, 체력 변수
    public int Level = 0;
    public int Kills = 0;
    public int Exp = 0;
    public int[] Next_exp = {10, 30, 60, 100, 140, 200, 260};
    public float P_Current_hp;
    public float P_Max_hp;

    //선택한 Player의 종류 구분을 위한 변수
    public int Player_idx;

    [Header("Game Object")]
    public float Game_time;
    public float Max_game_time = 100f;

    public bool Pause = true;

    public GameObject EnemyCleaner;

    public Result Result_UI;


    private void Awake() {
        
        instance = this;

        Time.timeScale = 0;

    }

    


    // Start is called before the first frame update
    public void GameStart(int idx)
    {
        //선택한 Player의 idx 받기
        Player_idx = idx;

        //현재 체력을 최대 체력으로 초기화
        P_Current_hp = P_Max_hp;

        //선택한 Player 생성시키기
        Player.gameObject.SetActive(true);

        //LevelUpUI_control의 Select함수 실행, 0을 넣으면 기본 무기 Shovel 장착
        LevelUpUI.Select(Player_idx % 2);

        //Game 시작을 위해 timescale 변경하는 Resume_game 함수 실행
        Resume_game();

    }

    //GameOver을 담당하는 함수
    public void GameOver() {

        StartCoroutine(GameOverRoutine());

    }

    //Game을 중지시키고, Result_UI를 화면에 띄우는 IEnumerator
    IEnumerator GameOverRoutine() {

        //일시정지 변수 활성화
        Pause = true;

        yield return new WaitForSeconds(0.5f);


        //Reuslt_UI 활성화 및 이미지 띄우기
        Result_UI.gameObject.SetActive(true);

        Result_UI.Lose();

        Stop_game();

    }

    //GameWin, 생존에 성공했을 경우를 담당하는 함수
    public void GameWin() {

        StartCoroutine(GameWinRoutine());

    }

    //Game을 중지시키고, Result_UI를 화면에 띄우는 IEnumerator
    IEnumerator GameWinRoutine() {

        //일시정지 변수 활성화
        Pause = true;

        //모든 Enemy를 사망 처리시키는 큰 총알인 EnemyCleaner 활성화
        EnemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        //모든 Enemy를 사망 처리시키는 큰 총알인 EnemyCleaner 활성화
        EnemyCleaner.SetActive(false);


        //Reuslt_UI 활성화 및 이미지 띄우기
        Result_UI.gameObject.SetActive(true);

        Result_UI.Win();

        Stop_game();

    }




    //Restart 를 담당하는 함수
    public void Restart(){

        
        SceneManager.LoadScene(0);

         

    }




    // Update is called once per frame
    void Update()
    {
        //Pause 상태라면, Update 함수를 돌지 않게 return해줌
        if (Pause) {
            return;
        }

        Game_time += Time.deltaTime;

        //Max_game_time 까지 버티면,
        if (Game_time > Max_game_time) {

            //Win 화면 띄우기
            Game_time = Max_game_time;
            GameWin();

        }

    }

    //경험치 획득을 관리하는 함수
    public void Get_exp() {

        //Pause 시에는, 경험치 획득을 방지함
        if (Pause) {
            return;
        }



        Exp += 1;

        //무한 레벨 업 기능을 위해, indexoutofrange 오류를 방지하고자 Mathf.Min 사용
        //Level이 Next_exp.Length 이상이 되도, Mathf.Min 에 의해 Netx_ext의 마지막 idx의 경험치 량으로 계산
        if(Exp == Next_exp[Mathf.Min(Level, Next_exp.Length - 1)]) {
            Level += 1;
            Exp = 0;

            //레벨업 시 능력 업그레이드를 선택하는 UI를 보여주는 함수
            //LevelUpUI_control에 있음
            LevelUpUI.Show_UI();
        }

    }


    //TimeScale을 통해 Unity 게임의 시간의 속도를 조절
    //게임의 시간을 멈추는 함수
    public void Stop_game() {
        
        //
        Pause = true;
        Time.timeScale = 0;


    }

    //게임의 시간을 다시 흐르게 하는 함수
    public void Resume_game() {

        Pause = false;
        Time.timeScale = 1;

    }


}
