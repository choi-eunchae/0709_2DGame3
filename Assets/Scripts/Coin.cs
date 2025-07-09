using UnityEditor.Build.Content;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Jump();
    }


    void Jump()
    {
        rb.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(3, 6)), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.ShowCoinCount(); // 코인 개수 표시
            Destroy(gameObject); // 코인 오브젝트 삭제
        }
    }
}
