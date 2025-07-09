using UnityEngine;

public class Background : MonoBehaviour
{   
    // 배경이 이동하는 속도 (초당 단위)
    [SerializeField]
    private float moveSpeed = 1f;

    // 매 프레임마다 호출됨
    void Update()
    {   
        // 배경이 아래로 이동
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        // 배경이 y = -10보다 아래로 내려가면, y축을 20만큼 올려서 반복효과
        if (transform.position.y < -10f)
        {
            transform.position += new Vector3(0, 20f, 0);
        }
    }
}
