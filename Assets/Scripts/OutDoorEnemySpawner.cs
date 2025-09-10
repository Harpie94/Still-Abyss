using UnityEngine;
using System.Collections;

public class OutdoorEnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] windowSpawnPoints;

    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Respawn")]
    public bool respawn = true;
    public float respawnDelay = 5f;

    public GameObject CurrentEnemy { get; private set; }

    private void Start()
    {
        if (windowSpawnPoints == null || windowSpawnPoints.Length == 0)
        {
            Debug.LogError("[EnemySpawner] Aucun spawn point assigne");
            return;
        }
        if (enemyPrefab == null)
        {
            Debug.LogError("[EnemySpawner] enemyPrefab non assigne");
            return;
        }

        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        int idx = Random.Range(0, windowSpawnPoints.Length);
        Transform p = windowSpawnPoints[idx];

        CurrentEnemy = Instantiate(enemyPrefab, p.position, p.rotation);
    }

    public void ClearCurrentEnemy()
    {
        if (CurrentEnemy != null)
        {
            Destroy(CurrentEnemy);
            CurrentEnemy = null;

            if (respawn)
                StartCoroutine(RespawnAfterDelay());
        }
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnEnemy();
    }
}
