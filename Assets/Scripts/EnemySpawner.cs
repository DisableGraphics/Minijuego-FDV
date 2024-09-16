using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject asteroidPrefab;
    public float spawnRatePerMinute = 30f;
	public float spawnRateIncrement = 1f;
	public float xLimit = 10f;
	public float maxLife = 1f;
	private float spawnNext = 0f;

	private class Use {
		public GameObject meteor;
		public bool is_alive = false;
		public float counter = 0.0f;
	}

	// Just in cause someone is crazy enough to manage 10 meteors
	private Use[] meteors = new Use[10];

	void Start() {
		for(int i = 0; i < meteors.Length; i++) {
			meteors[i] = new();
			meteors[i].meteor = Instantiate(asteroidPrefab, new(-120, -120), Quaternion.identity);
			meteors[i].meteor.SetActive(false);
		}
	}

	int Search(GameObject meteor) {
		for(int i = 0; i < meteors.Length; i++) {
			if(meteors[i].meteor == meteor) return i;
		}
		return -1;
	}

	public void RemoveMeteor(GameObject possibleMeteor) {
		int i = Search(possibleMeteor);
		if(i != -1) {
			ReturnToPool(i);
		}
	}

	void ReturnToPool(int i) {
		meteors[i].is_alive = false;
		meteors[i].meteor.SetActive(false);
	}

	void UpdateMeteors() {
		for(int i = 0; i < meteors.Length; i++) {
			Use mt = meteors[i];
			if(mt.is_alive) {
				mt.counter -= Time.deltaTime;
				if(mt.counter <= 0) {
					ReturnToPool(i);
				}
			}
		}
	}
	
	int GetFirstUnused() {
		for(int i = 0; i < meteors.Length; i++) {
			if(!meteors[i].is_alive) return i;
		}
		return -1;
	}

	void SpawnMeteor(int n) {
		if(n != -1) {
			meteors[n].is_alive = true;
			float rand = Random.Range(-xLimit, xLimit);
			Vector3 spawnPosition = new(rand, 8, -1);
			meteors[n].meteor.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
			meteors[n].meteor.SetActive(true);
			meteors[n].counter = maxLife;

			RigidbodyReset(meteors[n].meteor);
		}
	}

	void RigidbodyReset(GameObject meteorito) {
		Rigidbody rb = meteorito.GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
    	rb.angularVelocity = Vector3.zero;
	}

    // Update is called once per frame
    void Update()
    {
        if(Time.time > spawnNext) {
			spawnNext = Time.time + 60/spawnRatePerMinute;
			spawnRatePerMinute += spawnRateIncrement;
			SpawnMeteor(GetFirstUnused());
		}
		UpdateMeteors();
    }
}
