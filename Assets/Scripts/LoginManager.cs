using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 로그인과 회원가입을 관리하는 스크립트

public class LoginManager : MonoBehaviour
{
    [Header("로그인 UI")]
    public GameObject LoginScreen;          // 로그인 화면
    public TMP_InputField loginIDInput;     // 로그인 ID 입력창
    public TMP_InputField loginPasswordInput; // 로그인 비밀번호 입력창

    [Header("회원가입 UI")]
    public GameObject SignUpScreen;         // 회원가입 화면
    public TMP_InputField signupIDInput;    // 회원가입 ID 입력창
    public TMP_InputField signupNameInput;  // 회원가입 이름 입력창
    public TMP_InputField signupPasswordInput;    // 회원가입 비밀번호 입력창
    public TMP_InputField signupPasswordConfirmInput; // 비밀번호 확인 입력창

    [Header("알림창")]
    public GameObject ErrorAlert;           // 에러 알림창
    public TextMeshProUGUI ErrorAlertText;  // 에러 메시지

    void Start()
    {
        // 게임 시작 시 로그인 화면 보여주기
        ShowLoginScreen();
    }

    // ============== 화면 전환 함수들 ==============

    // 로그인 화면 보여주기
    public void ShowLoginScreen()
    {
        LoginScreen.SetActive(true);
        SignUpScreen.SetActive(false);
        HideErrorAlert();
    }

    // 회원가입 화면 보여주기
    public void ShowSignUpScreen()
    {
        LoginScreen.SetActive(false);
        SignUpScreen.SetActive(true);
        HideErrorAlert();
    }

    // ============== 로그인 기능 ==============

    // 로그인 버튼을 눌렀을 때
    public void LoginButton()
    {
        string inputID = loginIDInput.text;         // ID 입력창에서 글자 가져오기
        string inputPassword = loginPasswordInput.text; // 비밀번호 입력창에서 글자 가져오기

        // 빈칸 체크
        if (inputID == "" || inputPassword == "")
        {
            ShowErrorAlert("ID와 비밀번호를 입력해주세요!");
            return;
        }

        // 저장된 사용자 데이터 찾기
        string savedData = PlayerPrefs.GetString(inputID, ""); // ID로 데이터 찾기

        if (savedData == "") // 저장된 데이터가 없으면
        {
            ShowErrorAlert("존재하지 않는 ID입니다!");
            return;
        }

        // JSON을 UserData로 변환
        UserData userData = JsonUtility.FromJson<UserData>(savedData);

        // 비밀번호 확인
        if (userData.password != inputPassword)
        {
            ShowErrorAlert("비밀번호가 틀렸습니다!");
            return;
        }

        // 로그인 성공! 데이터 로드하고 ATM 화면으로 이동
        GameManager.Instance.userData = userData; // 현재 사용자 데이터 설정
        GameManager.Instance.userInfo.Refresh(); // UI 업데이트
        GameManager.Instance.AtmOn(); // ATM 화면 보여주기

        // 로그인 화면 끄기
        LoginScreen.SetActive(false);
        SignUpScreen.SetActive(false);
    }

    // ============== 회원가입 기능 ==============

    // 회원가입 버튼을 눌렀을 때
    public void SignUpButton()
    {
        string inputID = signupIDInput.text;
        string inputName = signupNameInput.text;
        string inputPassword = signupPasswordInput.text;
        string inputPasswordConfirm = signupPasswordConfirmInput.text;

        // 빈칸 체크
        if (inputID == "" || inputName == "" || inputPassword == "" || inputPasswordConfirm == "")
        {
            ShowErrorAlert("모든 정보를 입력해주세요!");
            return;
        }

        // 비밀번호 확인 체크
        if (inputPassword != inputPasswordConfirm)
        {
            ShowErrorAlert("비밀번호가 일치하지 않습니다!");
            return;
        }

        // 이미 존재하는 ID인지 체크
        string existingData = PlayerPrefs.GetString(inputID, "");
        if (existingData != "")
        {
            ShowErrorAlert("이미 존재하는 ID입니다!");
            return;
        }

        // 새 사용자 데이터 만들기 (기본 잔액: 50,000, 기본 현금: 100,000)
        UserData newUser = new UserData(inputID, inputPassword, inputName, 100000, 50000);

        // JSON으로 변환해서 저장
        string json = JsonUtility.ToJson(newUser);
        PlayerPrefs.SetString(inputID, json); // ID를 키로 사용해서 저장
        PlayerPrefs.Save();

        // 회원가입 성공 메시지
        ShowErrorAlert("회원가입이 완료되었습니다!");

        // 입력창 비우기
        signupIDInput.text = "";
        signupNameInput.text = "";
        signupPasswordInput.text = "";
        signupPasswordConfirmInput.text = "";

        Debug.Log("회원가입 성공: " + newUser.userName);

        // 2초 후 로그인 화면으로 이동
        Invoke("ShowLoginScreen", 2f);
    }

    // 회원가입 취소 버튼
    public void CancelSignUp()
    {
        ShowLoginScreen();
    }

    // ============== 알림창 관리 ==============

    // 에러 알림창 보여주기
    public void ShowErrorAlert(string message)
    {
        ErrorAlert.SetActive(true);
        ErrorAlertText.text = message;
    }

    // 에러 알림창 끄기
    public void HideErrorAlert()
    {
        ErrorAlert.SetActive(false);
    }
}
