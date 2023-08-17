using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    //GameOver, Survived 이미지를 가진 GameObject를 담을 배열
    public GameObject[] Titles;

    //패배시와 승리 시, 그에 맞는 GameObject 활성화, 0은 패배, 1은 승리
    public void Lose() {
        Titles[0].SetActive(true);
    }
    
    public void Win() {
        Titles[1].SetActive(true);
    }



}
