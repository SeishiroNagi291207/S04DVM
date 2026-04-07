using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public GameObject prefabEnemy;
    public Transform spawnPoint;
    public float spawnInterval = 3f;
    public int maxEnemys = 4;
    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;
        }
    }
    public void SpawnPrefab()
    {
        int current = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (current >= maxEnemys) return;
        Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
        GameObject go = Instantiate(prefabEnemy, pos, Quaternion.identity);
        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) enemy.Player = player.transform;
        }
    }
}
