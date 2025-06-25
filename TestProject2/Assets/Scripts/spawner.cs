using System.Runtime.CompilerServices;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject spawnerEntity;
    [SerializeField] int spawnAmount;
    [SerializeField] int spawnRate;
    [SerializeField] Transform[] spawnPos;

    float spawnTimer;
    int spawnCount;
    bool spawnerEnabled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.instance.UpdateGameGoal(spawnAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerEnabled) {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate && spawnCount < spawnAmount)
            {
                spawn();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnerEnabled = true;
        }
    }

    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);

        Instantiate(spawnerEntity, spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}
