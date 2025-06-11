using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 이름과 현금, 잔액을 표시하는 UI 컴포넌트를 관리하는 스크립트입니다.

public class UserInfo : MonoBehaviour
{
    public TextMeshProUGUI nameText;// 사용자 이름을 표시할 UI 텍스트 컴포넌트입니다.
    public TextMeshProUGUI cashText;// 사용자의 현금을 표시할 UI 텍스트 컴포넌트입니다.
    public TextMeshProUGUI balanceText;// 사용자의 잔액을 표시할 UI 텍스트 컴포넌트입니다.

    void Start()
    {
        // 게임 시작할 때 UI 업데이트
        Refresh();
    }

    // 새로운 데이터를 UI에 보여주는 함수
    public void Refresh()
    {
        // GameManager에서 사용자 데이터 가져오기
        UserData userData = GameManager.Instance.userData;

        // UI에 데이터 표시하기
        nameText.text = userData.userName;
        cashText.text = userData.cash.ToString("N0");
        balanceText.text = userData.balance.ToString("N0");
        // "N0" 포맷은 천 단위 구분 기호를 사용하여 숫자를 표시합니다.

        if (GameManager.Instance == null || GameManager.Instance.userData == null)
        {
            Debug.Log("GameManager 또는 userData가 null입니다!");
            return;
        }

    }
}