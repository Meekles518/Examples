using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_control : MonoBehaviour
{
    //Spawn 지점을 저장할 변수
    public Transform[] Spawn_point;

    //Spawn 되는 Unit의 data를 담을 list
    public Spawn_data[] Spawn_data;



    //Spawn 시간을 설정할 변수
    float Timer;

    //시간의 흐름에 따라 증가할 level 
    int level;
    float level_up = 10f;


    private void Awake() {

        //Children, 자식 개체들의 Transform components 를 가져오기
        //주의할 것은, 자기 자신의 Transform Component가 첫번째 idx로 들어간다
        //즉, Spawn_point[0] = Spawn_control의 Transform이 된다.
        Spawn_point = GetComponentsInChildren<Transform>();

    }
     

    // Update is called once per frame
    void Update()
    {

        //GameManager의 Pause(게임의 일시정지 유무)를 확인하고, Pause시 Update문 못들어가게 return
        if (GameManager.instance.Pause) {
            return;
        }

        //게속해서 시간을 더해주기, 시간이 지나면 지날수록 소환주기 빨라지게 하게끔
        Timer += Time.deltaTime;

        //시간의 흐름에 따라, level이 올라가게 하기
        level = Mathf.FloorToInt(GameManager.instance.Game_time / level_up);
        
        //IndexOutofRange를 방지하기 위해 idx 관리하기
        if (level >= Spawn_data.Length) {

            //level이 저장한 Spawn_data(Enemy의 종류가 들어있음) 길이보다 커지면, 강제로 idx맞추기
            level = Spawn_data.Length - 1;
        }

        if (Timer > Spawn_data[level].Spawn_time) {
            Spwan();
            Timer = 0f;
        }


        
    }


    //Pools에 있는 Enemy를 랜덤으로 Spawn하는 함수
    void Spwan() {

        //PoolManager의 Get함수에서, Instantiate 한 값을 return 받는다.
        //그 Instantiate한 Unit을 Enemy에 저장한다. 
        GameObject Enemy =  GameManager.instance.Pool.Get(0);

        //변수에 저장한 Instantiate 된 Unit의 좌표를 랜덤하게 변경해준다.
        Enemy.transform.position = Spawn_point[Random.Range(1, Spawn_point.Length)].position;


        Enemy.GetComponent<Enemy_control>().Get_data(Spawn_data[level]);

    }


}

//class를 Inspector에서 볼 수 있게 Serialize
[System.Serializable]
//Spawn의 data를 관리할 Class
public class Spawn_data{

    public int Sprite_type;

    public float Spawn_time;

    public int Hp;

    public float E_speed;





}




