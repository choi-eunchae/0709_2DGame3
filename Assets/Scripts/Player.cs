using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField]
    float moveSpeed = 1f;

    // 현재 미사일 프리팹 인덱스
    int missIndex = 0;

    // 미사일 프리팹 배열
    public GameObject[] missilePrefab;

    // 미사일 생성 위치
    public Transform spPostion;

    // 미사일 발사 간격(초)
    [SerializeField]
    private float shootInverval = 0.05f;

    // 마지막 발사 시간
    private float lastshotTime = 0f;

    // 애니메이터 컴포넌트 참조
    private Animator animator;

    public float tripleShotCooldown = 3.0f; // 세 방향 쿨타임
    public float tripleShotDuration = 5.0f; // 세 방향 연속 발사 시간
    public bool isTripleShooting = false; // 세 방향 연속 발사 상태
    public float tripleShotEndTime = 0f; // 세 방향 연속 발사 종료 시간
    public float lastTripleShotTime = -999f;

    public bool IsTripleShooting => isTripleShooting;



    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    // 매 프레임마다 이동 및 발사 처리
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Debug.Log("Horizontal Input: " + horizontalInput); // 디버그용 로그 출력
        Vector3 moveTo = new Vector3(horizontalInput, 0, 0);
        transform.position += moveTo * moveSpeed * Time.deltaTime; // 좌우 이동



        // 애니메이션 상태 변경
        if (horizontalInput < 0)
        {
            animator.Play("Left"); // 왼쪽 이동 애니메이션
        }
        else if (horizontalInput > 0)
        {
            animator.Play("Right"); // 오른쪽 이동 애니메이션
        }
        else
        {
            animator.Play("Idle"); // 가운데(정지) 애니메이션
        }
        Shoot(); // 미사일 발사

        if (isTripleShooting && Time.time >= tripleShotEndTime)
        {
            isTripleShooting = false;
            lastTripleShotTime = Time.time; // 쿨타임 시작 시점 갱신
        }
        GameManager.Instance.ShowSpecialCool(TripleShotCooldownRemaining);
        GameManager.Instance.UpdateTripleShotUI();


    }

    // 미사일 발사 함수
    void Shoot()
    {
        // UpArrow 누르면 세 방향 모드 활성화 (쿨타임 확인)
        if (Input.GetKeyDown(KeyCode.UpArrow))  // 키를 처음 눌렀을 때만 실행
        {
            if (!isTripleShooting && Time.time - lastTripleShotTime >= tripleShotCooldown)
            {
                isTripleShooting = true;
                tripleShotEndTime = Time.time + tripleShotDuration;
                lastTripleShotTime = Time.time; // 쿨타임 갱신
            }
        }

        // 세 방향 연속 발사 모드
        if (isTripleShooting && Time.time - lastshotTime > shootInverval)
        {
            FireTripleShot(); // 세 방향으로 발사
            lastshotTime = Time.time;
        }

        if (Time.time - lastshotTime > shootInverval)
        {
            Instantiate(missilePrefab[missIndex], spPostion.position, Quaternion.identity);
            lastshotTime = Time.time; // 미사일 발사 시간 갱신
        }
    }

    // 미사일 업그레이드 함수
    public void MissileUp()
    {
        missIndex++; // 미사일 종류 업그레이드
        shootInverval = shootInverval - 0.1f; // 발사 간격 감소(더 빠르게)
        if (shootInverval < 0.1f)
            shootInverval = 0.1f;
        if (missIndex >= missilePrefab.Length)
            missIndex = missilePrefab.Length - 1;
    }

    // 세 방향 발사 함수
    void FireTripleShot()
    {
        // 발사 각도
        float[] angles = { 90f, 140f, 35f }; // 가운데, 왼쪽, 오른쪽

        foreach (float angle in angles)
        {
            // 각도 계산
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 dir = new Vector3(dirX, dirY, 0).normalized;

            // 미사일 생성 및 속도 적용
            GameObject bullet = Instantiate(missilePrefab[missIndex], spPostion.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f; // 5f는 미사일 속도
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) // 몬스터 태그로 비교
        {
            Destroy(gameObject); // 플레이어 제거        
            GameManager.Instance.GameOver();
        }
    }

    public float TripleShotCooldownRemaining
    {
        get
        {
            float elapsed = Time.time - lastTripleShotTime;
            return Mathf.Max(0, tripleShotCooldown - elapsed);
        }
    }

    public float TripleShotDurationRemaining
    {
        get
        {
            if (!isTripleShooting) return 0f;
            return Mathf.Max(0, tripleShotEndTime - Time.time);
        }
    }
}
