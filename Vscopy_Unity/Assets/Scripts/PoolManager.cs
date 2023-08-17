using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    
    //Prefab을 보관할  변수
    public GameObject[] Prefabs; 

    //Pool 을 담을 List변수
    List<GameObject>[] Pools;



// pool = [[Red], [Blue]]


    //Pools list 초기화
    private void Awake() {

        Pools = new List<GameObject>[Prefabs.Length];

        for (int i = 0; i < Pools.Length; i++) {

            Pools[i] = new List<GameObject>();

        }
    }

    //GameObject를 return해주는 함수 
    public GameObject Get(int idx) {

        GameObject Select = null;

        //Pools[idx]에 대해서 E로 접근
        foreach (GameObject E in Pools[idx]) {

            //E가 비활설화 되어있는 경우
            if (!E.activeSelf) {

                //E를 SetActive로 활성화
                Select = E;
                Select.SetActive(true);
                break;
            }
        }


        //Select의 값이 없을 경우
        if (!Select) {

            //Instantiate로 Prefabs[idx]에 있는 원소를 생성하고,
            Select = Instantiate(Prefabs[idx], transform);

            //Pools에 Add해주기
            Pools[idx].Add(Select);

        }

        return Select;

    }

// pool = [[Red *], [Blue]]


}
