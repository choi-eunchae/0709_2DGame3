using UnityEngine;

public class Missile : MonoBehaviour
{   
    // 미사일 이동 속도
    [SerializeField]
    private float moveSpeed = 1f;

    // 미사일 데미지
    [SerializeField]
    public int missileDamege = 1;

    // 폭발 이펙트 프리팹
    [SerializeField]
    GameObject Expeffect;

    // 매 프레임마다 미사일을 위로 이동, 화면 밖으로 나가면 삭제
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        if (transform.position.y > 7f)
        {
            Destroy(this.gameObject);
        }
    }

    // 적과 충돌 시 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Missile hit enemy!"); // 디버그용 로그 출력
            GameObject effect = Instantiate(Expeffect, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // 1초 후 이펙트 삭제
            Destroy(gameObject); // 미사일 삭제
        }
    }
}
