using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemiesPrefab = new GameObject[3];
    public float[] enemiesSpawnInterval = new float[3] { 1, 3, 6 };
    public float[] enemiesXLimit;
    
    public GameObject[] awardsPrefab = new GameObject[2];
    public float[] awardsSpawnInterval = new float[2] { 8, 16 };
    
    void Start()
    {
        enemiesXLimit = new float[3] { Constants.SmallEnemyXLimit, Constants.MiddleEnemyXLimit, Constants.LargeEnemyXLimit };
        StartCoroutine(SpawnEnemy(1, 0));
        StartCoroutine(SpawnEnemy(1, 1));
        StartCoroutine(SpawnEnemy(1, 2));

        StartCoroutine(SpawnAward(1, 0));
        StartCoroutine(SpawnAward(1, 1));
    }
    
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy(float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            float x = Random.Range(-enemiesXLimit[index], enemiesXLimit[index]);
            Instantiate(enemiesPrefab[index], new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
            yield return new WaitForSeconds(enemiesSpawnInterval[index]);
        }
    }
    
    IEnumerator SpawnAward(float delay, int index)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            float x = Random.Range(-Constants.AwardXLimit, Constants.AwardXLimit);
            Instantiate(awardsPrefab[index], new Vector3(x, transform.position.y, transform.position.z), transform.rotation);
            yield return new WaitForSeconds(awardsSpawnInterval[index]);
        }
    }
}
