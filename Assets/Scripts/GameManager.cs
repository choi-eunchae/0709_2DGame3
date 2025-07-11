using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int coin = 0; // 현재 코인 개수
    public TextMeshProUGUI textMeshProCoin; // 코인 개수를 표시할 텍스트
    public static GameManager Instance { get; private set; } // 싱글톤 인스턴스
    public GameObject gameOverPanel; // 게임 오버 UI 패널
    public GameObject gameClearPanel; // 게임 클리어 UI 패널
    public GameObject retryButton;
    public TextMeshProUGUI[] top3Texts; // 탑3 기록 표시용 텍스트 배열
    public int clearCoinTarget = 80; // 클리어 조건 (100코인)
    public TextMeshProUGUI tripleShotTimerText; // 쿨타임 및 발사시간 표시용 텍스트
    public Image CoolTimePanel;



    void Awake()
    {
        // 싱글톤 인스턴스가 없으면 자신을 할당
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
            retryButton.SetActive(false);
            gameOverPanel.SetActive(false);
            if (gameClearPanel != null)
                gameClearPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 중복 파괴
            Instance = this;     // 새 인스턴스로 교체
        }
    }


    // 코인 개수를 증가시키고 UI에 표시, 2개마다 미사일 업그레이드
    public void ShowCoinCount()
    {
        coin++;
        textMeshProCoin.SetText(coin.ToString()); // 코인 개수 UI 갱신
        Debug.Log("Coin increased! 현재 코인: " + coin);
        // 코인이 2의 배수일 때마다 미사일 업그레이드
        if (coin % 2 == 0)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.MissileUp(); // 2개마다 미사일 업그레이드
            }
        }
        // 클리어 조건 체크
        if (coin >= clearCoinTarget)
        {
            Debug.Log("클리어 조건 달성!"); // 추가
            GameClear();
        }
    }

    // 게임 오버 처리
    public void GameOver()
    {
        Debug.Log("게임 오버!");
        gameOverPanel.SetActive(true);
        retryButton.SetActive(true);

        SaveScore(coin);
        DisplayTop3();

        Time.timeScale = 0f; // 게임 멈춤
    }

    // 게임 클리어 처리
    public void GameClear()
    {
        Debug.Log("게임 클리어!");
        if (gameClearPanel != null)
        {
            gameClearPanel.SetActive(true);
        }
        retryButton.SetActive(true);

        SaveScore(coin);
        DisplayTop3();

        Time.timeScale = 0f; // 게임 멈춤
    }

    void ShowRetryButton()
    {
        retryButton.SetActive(true);
    }


    void SaveScore(int score)
    {
        // 기존 탑3 점수 읽기
        int[] scores = new int[3];
        for (int i = 0; i < 3; i++)
        {
            scores[i] = PlayerPrefs.GetInt("TopScore" + i, 0);
        }

        // 현재 점수를 넣고 정렬
        System.Collections.Generic.List<int> scoreList = new System.Collections.Generic.List<int>(scores);
        scoreList.Add(score);
        scoreList.Sort();
        scoreList.Reverse(); // 내림차순 정렬

        // 상위 3개만 저장
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("TopScore" + i, scoreList[i]);
        }
        PlayerPrefs.Save();
    }

    void DisplayTop3()
    {
        for (int i = 0; i < 3; i++)
        {
            int score = PlayerPrefs.GetInt("TopScore" + i, 0);
            top3Texts[i].SetText($"{i + 1}위: {score} 코인");
        }
    }

    public void Retry()
    {
        Time.timeScale = 1f; // 멈춘 시간 되돌리기
        coin = 0; // 코인 리셋 (필요 시)
        Debug.Log("게임 다시 시작!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }
    public void ShowSpecialCool(float Cooltime)
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        CoolTimePanel.fillAmount = Cooltime / player.tripleShotCooldown;
    }

    public void UpdateTripleShotUI()
    {
        if (tripleShotTimerText == null) return;

        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        if (player.isTripleShooting)
        {
            float remain = player.TripleShotDurationRemaining;
            tripleShotTimerText.SetText($"Special Missile: {remain:F1}");
        }
        else
        {
            float remain = player.TripleShotCooldownRemaining;
            if (remain > 0)
                tripleShotTimerText.SetText($"CoolTime: {remain:F1}");
            else
                tripleShotTimerText.SetText("Special Ready!");
        }
    }
}
