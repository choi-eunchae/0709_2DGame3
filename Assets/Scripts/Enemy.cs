using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject damageEffectPrefab;
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private Color originalColor;
    public float enemyHp = 1;

    [SerializeField]
    public float moveSpeed = 1f;
    public GameObject Coin;
    public GameObject Effect;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    // // 적이 파격 시 깜빡임 효과
    // public void flash()
    // {
    //     StopAllCoroutines(); // 기존 코루틴 중지
    //     StartCoroutine(FlashRoutine());
    // }

    // // 파격 시 색상 변경 코루틴
    // private IEnumerator FlashRoutine()
    // {
    //     spriteRenderer.color = flashColor;
    //     yield return new WaitForSeconds(flashDuration);
    //     spriteRenderer.color = originalColor;
    // }

    // 이동 속도 설정
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    // 매 프레임마다 아래로 이동, 화면 밖으로 나가면 삭제
    void Update()
    {
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    // 미사일과 충돌 시 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            Missile missile = collision.GetComponent<Missile>();
            StopAllCoroutines(); // 기존 코루틴 중지
            StartCoroutine("HitColor"); // 파격 색상 코루틴 실행

            enemyHp = enemyHp - missile.missileDamege; // 체력 감소
            if (enemyHp < 0)
            {
                Destroy(gameObject); // 적 삭제
                Instantiate(Coin, transform.position, Quaternion.identity); // 코인 생성
                Instantiate(Effect, transform.position, Quaternion.identity); // 이펙트 생성
            }
            TakeDamage(missile.missileDamege); // 데미지 팝업 표시
        }
    }

    // 파격 시 색상 변경 코루틴
    IEnumerator HitColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    // 데미지 팝업 표시 함수
    void TakeDamage(int damage)
    {
        DamagePopupManager.Instance.CreateDamageText(damage, transform.position);
    }
}
