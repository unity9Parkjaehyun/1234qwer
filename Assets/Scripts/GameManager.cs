using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 인스턴스
    public UserData userData; // 사용자 데이터

    public GameObject DepositUIScreen;
    public GameObject WithdrawUIScreen;
    public GameObject TransferUIScreen;  // 송금 화면 (새로 추가)
    public GameObject Backbutton;
    public GameObject Atm;

    // 알림창과 UI들
    public GameObject MoneyAlert; // 돈 부족 알림창
    public TextMeshProUGUI MoneyAlertText; // 알림 메시지
    public UserInfo userInfo; // UI 업데이트용
    public TMP_InputField depositInputField; // 입금용 입력창
    public TMP_InputField withdrawInputField; // 출금용 입력창

    // 송금용 입력창들 (새로 추가)
    public TMP_InputField transferTargetInput; // 송금 대상 ID 입력창
    public TMP_InputField transferAmountInput; // 송금 금액 입력창

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // 싱글톤 인스턴스 설정
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 객체 파괴
        }
        userData = new UserData("박재현", 100000, 50000); // 기본 사용자 데이터 설정

        // 게임 시작 시 ATM 화면 끄기 (로그인 화면만 보이게)
        Atm.SetActive(false);
        DepositUIScreen.SetActive(false);
        WithdrawUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false);
        Backbutton.SetActive(false);
    }

    // ============== UI 화면 전환 함수들 ==============

    public void AtmOn()
    {
        Atm.SetActive(true);
        userInfo.gameObject.SetActive(true); // UserInfo도 켜기!
        DepositUIScreen.SetActive(false);
        WithdrawUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false); // 송금 화면도 끄기
        Backbutton.SetActive(false);
    }

    public void AtmOff()
    {
        Atm.SetActive(false);
        DepositUIScreen.SetActive(true);
        WithdrawUIScreen.SetActive(true);
        TransferUIScreen.SetActive(true); // 송금 화면도 켜기
        Backbutton.SetActive(true);
    }

    public void DepositUIScreenOn()
    {
        Atm.SetActive(false);
        DepositUIScreen.SetActive(true);
        WithdrawUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false);
        Backbutton.SetActive(true);
    }

    public void DepositUIScreenOff()
    {
        Atm.SetActive(true);
        DepositUIScreen.SetActive(false);
        WithdrawUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false);
        Backbutton.SetActive(false);
    }

    public void WithdrawUIScreenOn()
    {
        Atm.SetActive(false);
        WithdrawUIScreen.SetActive(true);
        DepositUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false);
        Backbutton.SetActive(true);
    }

    public void WithdrawUIScreenOff()
    {
        Atm.SetActive(true);
        WithdrawUIScreen.SetActive(false);
        DepositUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false);
        Backbutton.SetActive(false);
    }

    // 송금 화면 켜기 (새로 추가)
    public void TransferUIScreenOn()
    {
        Atm.SetActive(false);
        TransferUIScreen.SetActive(true);
        DepositUIScreen.SetActive(false);
        WithdrawUIScreen.SetActive(false);
        Backbutton.SetActive(true);
    }

    // 송금 화면 끄기 (새로 추가)
    public void TransferUIScreenOff()
    {
        Atm.SetActive(true);
        TransferUIScreen.SetActive(false);
        DepositUIScreen.SetActive(false);
        WithdrawUIScreen.SetActive(false);
        Backbutton.SetActive(false);
    }

    public void BackbuttonOn()
    {
        Atm.SetActive(true);
        DepositUIScreen.SetActive(false);
        WithdrawUIScreen.SetActive(false);
        TransferUIScreen.SetActive(false);
        Backbutton.SetActive(false);
    }

    public void BackbuttonOff()
    {
        Atm.SetActive(false);
        DepositUIScreen.SetActive(true);
        WithdrawUIScreen.SetActive(true);
        TransferUIScreen.SetActive(true);
        Backbutton.SetActive(true);
    }

    // ============== 입금 함수들 (현금 → 잔액) ==============

    public void Deposit(int amount)
    {
        if (userData.cash >= amount) // 현금이 유니티로 적은 값 만큼 있나?
        {
            userData.cash = userData.cash - amount; // 현금에서 유니티로 적은 값 만큼 빼기
            userData.balance = userData.balance + amount; // 잔액에 유니티로 적은 값 만큼 더하기
            userInfo.Refresh(); // 화면 업데이트
            SaveCurrentUserData(); // 현재 사용자 데이터 저장
        }
        else
        {
            ShowAlert("현금이 부족합니다!");
        }
    }

    // ============== 출금 함수들 (잔액 → 현금) ==============

    public void Withdraw(int amount)
    {
        if (userData.balance >= amount) // 잔액이 유니티로 적은 값 만큼 이상 있나?
        {
            userData.balance = userData.balance - amount; // 잔액에서 유니티로 적은 값 만큼 빼기
            userData.cash = userData.cash + amount; // 현금에 유니티로 적은 값 만큼 더하기
            userInfo.Refresh(); // 화면 업데이트
            SaveCurrentUserData(); // 현재 사용자 데이터 저장
        }
        else
        {
            ShowAlert("잔액이 부족합니다!");
        }
    }

    // ============== 직접 입력 기능 ==============

    // 입력창에 쓴 금액으로 입금
    public void DepositInputAmount()
    {
        string inputText = depositInputField.text; // 입금 입력창의 글자 가져오기

        // 입력창이 비어있으면
        if (inputText == "")
        {
            ShowAlert("금액을 입력해주세요!");
            return;
        }

        // 글자를 숫자로 바꾸기
        int amount = 0;
        bool canConvert = int.TryParse(inputText, out amount);

        // 숫자로 바뀌지 않거나 0 이하면
        if (canConvert == false || amount <= 0)
        {
            ShowAlert("올바른 금액을 입력해주세요!");
            return;
        }

        // 현금이 충분한지 확인
        if (userData.cash >= amount)
        {
            userData.cash = userData.cash - amount; // 현금에서 빼기
            userData.balance = userData.balance + amount; // 잔액에 더하기
            userInfo.Refresh(); // 화면 업데이트
            depositInputField.text = ""; // 입금 입력창 비우기
            SaveCurrentUserData(); // 현재 사용자 데이터 저장
        }
        else
        {
            ShowAlert("현금이 부족합니다!");
        }
    }

    // 입력창에 쓴 금액으로 출금
    public void WithdrawInputAmount()
    {
        string inputText = withdrawInputField.text; // 출금 입력창의 글자 가져오기

        // 입력창이 비어있으면
        if (inputText == "")
        {
            ShowAlert("금액을 입력해주세요!");
            return;
        }

        // 글자를 숫자로 바꾸기
        int amount = 0;
        bool canConvert = int.TryParse(inputText, out amount);

        // 숫자로 바뀌지 않거나 0 이하면
        if (canConvert == false || amount <= 0)
        {
            ShowAlert("올바른 금액을 입력해주세요!");
            return;
        }

        // 잔액이 충분한지 확인
        if (userData.balance >= amount)
        {
            userData.balance = userData.balance - amount; // 잔액에서 빼기
            userData.cash = userData.cash + amount; // 현금에 더하기
            userInfo.Refresh(); // 화면 업데이트
            withdrawInputField.text = ""; // 출금 입력창 비우기
            SaveCurrentUserData(); // 현재 사용자 데이터 저장
        }
        else
        {
            ShowAlert("잔액이 부족합니다!");
        }
    }

    // ============== 송금 기능 (새로 추가) ==============

    // 송금하기 버튼을 눌렀을 때
    public void TransferMoney()
    {
        string targetID = transferTargetInput.text; // 송금 대상 ID
        string amountText = transferAmountInput.text; // 송금 금액 (글자)

        // 빈칸 체크
        if (targetID == "" || amountText == "")
        {
            ShowAlert("입력 정보를 확인해주세요.");
            return;
        }

        // 금액을 숫자로 바꾸기
        int amount = 0;
        bool canConvert = int.TryParse(amountText, out amount);

        // 숫자로 바뀌지 않거나 0 이하면
        if (canConvert == false || amount <= 0)
        {
            ShowAlert("올바른 금액을 입력해주세요!");
            return;
        }

        // 내 잔액이 충분한지 확인
        if (userData.balance < amount)
        {
            ShowAlert("잔액이 부족합니다.");
            return;
        }

        // 송금 대상이 존재하는지 확인
        string targetData = PlayerPrefs.GetString(targetID, "");
        if (targetData == "")
        {
            ShowAlert("대상이 없습니다.");
            return;
        }

        // 자기 자신에게 송금하는지 확인
        if (targetID == userData.userID)
        {
            ShowAlert("자신에게는 송금할 수 없습니다!");
            return;
        }

        // 송금 대상 데이터 가져오기
        UserData targetUser = JsonUtility.FromJson<UserData>(targetData);

        // 송금 실행
        userData.balance = userData.balance - amount; // 내 잔액에서 빼기
        targetUser.balance = targetUser.balance + amount; // 상대방 잔액에 더하기

        // 데이터 저장
        SaveCurrentUserData(); // 내 데이터 저장
        SaveUserData(targetUser); // 상대방 데이터 저장

        // UI 업데이트
        userInfo.Refresh();

        // 입력창 비우기
        transferTargetInput.text = "";
        transferAmountInput.text = "";

        // 성공 메시지
        ShowAlert("송금이 완료되었습니다!");

        Debug.Log(amount + "원을 " + targetUser.userName + "에게 송금했습니다.");
    }

    // ============== 알림창 관리 ==============

    // 알림창 보여주기, 현금이 부족합니다! 같은 메세지를 화면에 띄워요 
    public void ShowAlert(string message)
    {
        MoneyAlert.SetActive(true); // 알림창 켜기
        MoneyAlertText.text = message; // 이 메세지를 알림창에 표시
    }

    // 알림창 끄기
    public void HideAlert()
    {
        MoneyAlert.SetActive(false); // 알림창 끄기
    }

    // ============== JSON 저장/로드 기능 ==============

    // 현재 사용자 데이터 저장하기
    public void SaveCurrentUserData()
    {
        if (userData.userID != "")
        {
            SaveUserData(userData);
        }
    }

    // 특정 사용자 데이터 저장하기
    public void SaveUserData(UserData user)
    {
        string json = JsonUtility.ToJson(user); // 데이터를 글자로 바꾸기
        PlayerPrefs.SetString(user.userID, json); // ID를 키로 사용해서 저장
        PlayerPrefs.Save(); // 완료
    }

    // 데이터 불러오기 (기존 호환용)
    public void LoadData()
    {
        string json = PlayerPrefs.GetString("SaveData", ""); // 저장된 데이터 가져오기

        if (json != "") // 저장된 데이터가 있으면
        {
            userData = JsonUtility.FromJson<UserData>(json); // 데이터 복원                      
            userInfo.Refresh(); // 화면 업데이트

        }
        else
        {
            Debug.Log("저장된 데이터 없음!");
        }
    }
}