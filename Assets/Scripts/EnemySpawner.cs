using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject asteroidPrefab;
    public float spawnRatePerMinute = 30f;
	public float spawnRateIncrement = 1f;
	public float xLimit = 10f;
	public float maxLife = 1f;
	private float spawnNext = 0f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > spawnNext) {
			spawnNext = Time.time + 60/spawnRatePerMinute;
			spawnRatePerMinute += spawnRateIncrement;

			float rand = Random.Range(-xLimit, xLimit);

			Vector3 spawnPosition = new(rand, 8, -1);

			GameObject meteor = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
			Destroy(meteor, maxLife);
		}
    }
}
