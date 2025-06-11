using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// 이 스크립트가 뭘 하는 애냐면..
// 사용자의 데이터(이름, 현금, 잔액)를 저장하고,
// 시작 시 해당 정보를 UI에 표시하는 역할을 합니다."

[System.Serializable] // 유니티에서 데이터를 저장/관리할 수 있도록

public class UserData
{
    public string userID;       // 사용자 ID (로그인용)
    public string password;     // 사용자 비밀번호 (로그인용)
    public string userName;     // 사용자 이름을 저장하는 변수입니다.
    public int cash;           // 사용자의 현금을 저장하는 변수입니다.
    public int balance;        // 사용자의 잔액을 저장하는 변수입니다.

    // 기본 생성자 (빈 데이터용)
    public UserData()
    {
        this.userID = "";
        this.password = "";
        this.userName = "";
        this.cash = 0;
        this.balance = 0;
    }

    // 전체 데이터 생성자
    public UserData(string userID, string password, string userName, int cash, int balance)
    {
        this.userID = userID;         // ID 초기화
        this.password = password;     // 비밀번호 초기화
        this.userName = userName;     // 생성자에서 사용자 이름을 초기화합니다.
        this.cash = cash;            // 생성자에서 현금을 초기화합니다.
        this.balance = balance;      // 생성자에서 잔액을 초기화합니다.
    }

    // 기존 호환용 생성자 (ID, 비밀번호 없이)
    // Constructor Overloading (생성자 오버로딩) - 같은 이름의 생성자를 여러 개 정의할 수 있습니다.
    // 잘 모른다면 이 링크를 참고하세요
    // https://claude.ai/share/67f3f0ed-8dae-4d0a-84d7-c3ca6b49eae7
    public UserData(string userName, int cash, int balance)
    {
        this.userID = "";
        this.password = "";
        this.userName = userName;
        this.cash = cash;
        this.balance = balance;
    }

    //  이 함수 안의 내용을 자연어로 쓰면 다음과 같습니다:
    // public class UserData 함수안에 필드를 만들었고
    // public UserData(string userName, int cash, int balance) 이 함수안에
    // 생성자를 정의하여 이를 통해 사용자 이름, 현금, 잔액을 초기화합니다.
    // 초기화란 변수에 처음 값을 넣어주는 것을 말합니다.
    // [System.Serializable] 로 유니티에서 데이터를 저장/관리할 수 있도록 짜놓았습니다.
    //또한 [System.Serializable]는 JSON 파일로 저장도 가능하고 게임저장/불러오기 기능도 구현이 가능합니다.


}


