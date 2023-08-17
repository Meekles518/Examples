using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_control : MonoBehaviour
{
    //무기의 Id, Prefab Id, dmg, 갯수, 속도를 저장할 변수
    public int Id;

    public int Prefabs_idx;

    public float Weapon_dmg;

    public int Count;

    public float Weapon_speed;

    //근접 무기의 경우, Player와 떨어진 거리를 저장할 변수
    public float Weapon_distance = 1.5f;


    //원거리 무기 발사 간격을 위해 시간을 저장할 변수
    float Fire_time = 0f;

    //
    Player_control Player_control;


    private void Awake() {

        //게임 시작 시, GamaManager을 통해 Player 정보 가져오기
        Player_control = GameManager.instance.Player;
    }


    // private void Start() {

        
    //     transform.position = new Vector3 (0, 0, 0);
         

    // }



    // Update is called once per frame
    void Update()
    {

        //GameManager의 Pause(게임의 일시정지 유무)를 확인하고, Pause시 Update문 못들어가게 return
        if (GameManager.instance.Pause) {
            return;
        }



        //Id에 따라 근접, 원거리 무기 구분해서 로직 적용
        switch (Id) {

            //근접무기일 경우, 회전시켜주자
            case 0:
                
                //Weapon을 Children으로 둔 부모를 회전시켜줘서, Weapon은 고정되어 있지만
                //Parent의 회전을 따라가기에, 회전하는 것 처럼 보임
                //Vector3.foward, 즉 Z축을 기준으로 회전시켜주자
                transform.Rotate(Vector3.forward * Weapon_speed * Time.deltaTime);

                break;
            

            default:

                //원거리 무기일 경우, 계속 시간 더해주기
                Fire_time += Time.deltaTime;

                //사격 할 시간이 되면 Fire함수 사용
                if (Fire_time > Weapon_speed) {
                    
                    //일정시간마다 함수가 사용되게 하기 위해 사격 시간 다시 초기화
                    Fire_time = 0f;
                    Fire();
                    
                }


                break;

        }
    }



    //Level up에 따른 Weapon을 강화시켜주는 함수
    public void Lvl_up(float Weapon_dmg, int count) {


        this.Weapon_dmg = Weapon_dmg;
        this.Count += count;

        // print(Id);
        // print(count);

        //Id == 0 인 근접무기의 경우, 갯수 증가 시 각도를 다시 맞춰주어야 하기에 함수를 다시 부르기
        if (Id == 0) {
            //print("Batch");
            Batch();
        }

        //레벨업 시, BroadcastMessage를 통해 Player의 모든 자식에게 Gear_update 함수를 실행하게 함
        //SemdMessageOptions를 통해, Player이 자식을 가지고 있지 않아도 에러를 안띄우게 함
        Player_control.BroadcastMessage("Gear_update" ,SendMessageOptions.DontRequireReceiver);

    }


    //무기의 Id값에 따라 로직이 다름
    public void Init(Itemdata Item_data) {

        //기본 셋, Hirarchy에 생성할 unit?의 이름 설정
        name = "Weapon" + Item_data.Item_id;

        //생성 시 Player 를 Parent로 하게 함
        transform.parent = Player_control.transform;
        //localPosition으로 Parent인 Player 기준으로 좌표 접근, 영벡터로 설정해
        //Player의 위치와 같게 함
        transform.localPosition = Vector3.zero;

        //Itemdata에 저장되어있는 값들로 초기화

        Id = Item_data.Item_id;

        //print("Init id");

        Count = Item_data.Base_cnt;

        Weapon_dmg = Item_data.Base_dmg;

        //PoolManager에 접근 시 사용하는 Prefab_idx의 값을 조정하기 위한 코드
        //근접무기와 원거리 무기 총알 생성을 위해서 Object Pooling을 이용함
        //PoolManager의 Get() 함수를 사용하기 위해서는, Prefabs_idx값을 넣어주어야 함
        for (int idx = 0; idx < GameManager.instance.Pool.Prefabs.Length; idx++) {

            //Itemdata에 설정한 Project_tile(Prefab을 넣어둠)과
            //PoolManager의 Prefabs[idx] 를 비교, 둘이 같다면 Prefabs_idx 초기화
            if (Item_data.Project_tile == GameManager.instance.Pool.Prefabs[idx]) {

                Prefabs_idx = idx;
                break;
            }
        }


        switch (Id) {
            
            //Id가 0이면(근접무기로 설정함) 무기 이동속도 
            case 0:
                //근접 무기의 경우, Speed는 회전 속도
                Weapon_speed -= 150;

                //재생성 시 회전체의 각도 조절을 위해 모든 회전체를 재 생성
                Batch();

                break;
            
            default:
                //원거리 무기의 경우, Speed는 연사 속도
                Weapon_speed = 0.3f;

                //Character의 특성 반영해주기
                Weapon_dmg *= Char_Adv.RangeWeaponDmg;
                break;


        }
        
        //열거형 enum의 원소들은, 정수의 idx로도 접근 가능
        //enum의 첫번째 원소는 0, 두번째는 1... 이런 식으로 저장되어 있음
        //int를 통해 형변환을 시키면 됨
        Hand_control Hand = Player_control.Hands[(int)Item_data.Item_type];


        //Init함수의 인자로 받은 Item_data의 Hand에 저장되어 있는 Sprite를 넣어주기
        Hand.Sprite.sprite = Item_data.Hand;

        //object 활성화 시키기
        Hand.gameObject.SetActive(true);


         //레벨업 시, BroadcastMessage를 통해 Player의 모든 자식에게 Gear_update 함수를 실행하게 함
        //SemdMessageOptions를 통해, Player이 자식을 가지고 있지 않아도 에러를 안띄우게 함
        Player_control.BroadcastMessage("Gear_update", SendMessageOptions.DontRequireReceiver);

    }



    //근접 Weapon을 관리하는 함수
    void Batch() {

        //무기의 개수만큼 for문 이용해서 생성
        for (int idx = 0; idx < Count; idx++) {

            Transform Bullet;

            //Lvl_up 함수를 통한 무기 개수 증가 시, 각도 재배치를 위한 구문
            //Count값이 증가하기 전에 이미 생성되어 있던 무기는, 변수에 저장만 하고 재활용
            if (idx < transform.childCount) {

                //Getchild를 통해 자식 개체 중 idx번째의 transform 정보를 가져오기
                Bullet = transform.GetChild(idx);

            }   

            //Count값의 증가로 추가로 생성해야 되는 무기는, Get함수 이용하기
            else {

                //PoolManager의 Get함수 이용, 무기 생성 하기
                Bullet = GameManager.instance.Pool.Get(Prefabs_idx).transform;

                 // //생성한 무기는 PoolManager가 부모로 되어 있기에, 부모를 이 Script를 가진 대상으로 바꾸기
                Bullet.parent = gameObject.transform;

            }

            //생성 시, LocalPosition과 LocalRotation을 초기화시켜주기
            //Local, 즉 World 좌표 기준이 아닌, Parent를 기준으로 (0, 0, 0)으로 위치 설정해주기
            Bullet.localPosition = Vector3.zero;
            Bullet.localRotation = Quaternion.identity;


            // //근접 무기의 경우, 여러개 있을 시 Player을 빙글빙글 도는 구조
            // //각 무기를 생성된 개수에 맞게 회전 시키기 위한 Z축 벡터값
            // //Ex) Count가 2(0도, 180도), 4(0도, 90도, 180도, 270도) 
            Vector3 Rotation_vec = Vector3.forward * 360 * idx / Count;


            // //생성 된 Bullet을 갯수에 따른 각도만큼 회전시키기
            Bullet.Rotate(Rotation_vec);


            // //Bullet은 생성 직후 Player의 자식 개체인 Weapon의 자식으로 생성됨
            // //즉, Bullet은 Player의 위치와 같은 좌표에 생성된다
            // //Player와 거리를 두기 위해, Bullet이 바라보는 윗방향, 
            // //즉 Bullet.up에 Weapon_distance(떨어트릴 거리)를 곱해 Bullet 기준 위로 이동
            // //
            //Bullet.Translate(Bullet.transform.up * Weapon_distance, Space.World);

            Bullet.Translate(Bullet.transform.up * Weapon_distance, Space.World);

            // //-1 은 근접 무기용, 관통 수 제한이 없음
            Bullet.GetComponent<Bullet_control>().Init(Weapon_dmg, -1, Vector3.zero);



        }

    }

    //원거리 무기 발사를 구현하는 함수
    void Fire() {

        //Scanner_control에 있는 Nearest_taregt, 일정 거리 이내의 Target이 없다면 무기 발사 안함
        if (!Player_control.Scanner_Control.Nearest_target) {
            return;
        }

        //Scanner_control에 저장되어 있는 Nearest_target의 위치를 가져오기
        Vector3 Target_pos = Player_control.Scanner_Control.Nearest_target.position;

        //Target과 Player의 벡터를 빼, Target으로 향하는 벡터 만들기
        Vector3 Target_direction = (Target_pos - transform.position).normalized;


        //발사할 원거리 무기를 PoolManger의 Get함수로 생성 및 객체 저장하기
        Transform Bullet = GameManager.instance.Pool.Get(Prefabs_idx).transform;

        //Player의 위치로 좌표 설정
        Bullet.position = transform.position;

        //FromToRotation 함수를 통해, Vector3.up 즉 Z축을 기준으로 Bullet을 회전시켜준다
        //얼마나? Target_direction, Target을 바라보게끔
        Bullet.rotation = Quaternion.FromToRotation(Vector3.up ,Target_direction);


        Bullet.GetComponent<Bullet_control>().Init(Weapon_dmg, Count, Target_direction);

    }








}
