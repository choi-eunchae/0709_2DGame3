using System.Collections;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject[] Enemies;
    float[] arrPosx = { -2f, 0f, 2f };
    [SerializeField]
    float spawnInterval = 0.5f;
    float moveSpeed = 5f;
    public Transform spawnPostision;
    int currentEnemyIndex = 0;
    int spawncount = 0;
    void Start()
    {
        StartCoroutine("EnemyRoutine");
    }

    IEnumerator EnemyRoutine()
    {
        yield return new WaitForSeconds(3);

        while(true)
        {
            for (int i = 0; i < arrPosx.Length; i++)
            {
                int spawnIndex;

                // 아직 배열 끝까지 안 갔으면 순서대로
                if (currentEnemyIndex < Enemies.Length)
                {
                    spawnIndex = currentEnemyIndex;
                }
                else
                {
                    // 배열 끝까지 갔으면 랜덤으로
                    spawnIndex = Random.Range(0, Enemies.Length);
                }

                SpawnEnemy(arrPosx[i], spawnIndex, moveSpeed);
            }

        spawncount++;

        if (spawncount % 2 == 0)
        {
            currentEnemyIndex++;
            moveSpeed += 2; // 속도 증가
        }
                yield return new WaitForSeconds(spawnInterval);
            
        }
    }

    // 적을 생성하는 함수
    void SpawnEnemy(float posX, int index, float moveSpeed)
    {
        Vector3 spawnPos = new Vector3(posX, spawnPostision.position.y, spawnPostision.position.z); // 생성 위치 계산

        GameObject enemyObject = Instantiate(Enemies[index], spawnPos, Quaternion.identity); // 적 생성
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        enemy.SetMoveSpeed(moveSpeed); // 이동 속도 설정
    }
}
