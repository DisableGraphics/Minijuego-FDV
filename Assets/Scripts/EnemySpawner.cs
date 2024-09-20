using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public GameObject asteroidPrefab;
	public GameObject childAsteroidPrefab;
    public float spawnRatePerMinute = 30f;
	public float spawnRateIncrement = 1f;
	public float xLimit = 10f;
	public float maxLife = 1f;
	private float spawnNext = 0f;
	public float launchForce = 5f;

	private class MeteorWrapper {
		public GameObject meteor;
		public bool is_alive = false;
		public float counter = 0.0f;
	}

	private class ChildMeteorWrapper {
		public GameObject childMeteor;
		public bool is_alive = false;
		public float counter = 0.0f;
	}

	// Just in cause someone is crazy enough to manage 10 meteors
	private MeteorWrapper[] meteors = new MeteorWrapper[10];
	private ChildMeteorWrapper[] childMeteors = new ChildMeteorWrapper[20];

	void Start() {
		for(int i = 0; i < meteors.Length; i++) {
			meteors[i] = new();
			meteors[i].meteor = Instantiate(asteroidPrefab, new(-120, -120), Quaternion.identity);
			meteors[i].meteor.SetActive(false);
		}
		for(int i = 0; i < childMeteors.Length; i++) {
			childMeteors[i] = new();
			childMeteors[i].childMeteor = Instantiate(childAsteroidPrefab, new(-120, -120), Quaternion.identity);
			childMeteors[i].childMeteor.SetActive(false);
		}
	}

	int SearchMeteor(GameObject meteor) {
		for(int i = 0; i < meteors.Length; i++) {
			if(meteors[i].meteor == meteor) return i;
		}
		return -1;
	}

	int SearchChildMeteor(GameObject meteor) {
		for(int i = 0; i < childMeteors.Length; i++) {
			if(childMeteors[i].childMeteor == meteor) return i;
		}
		return -1;
	}


	public void RemoveMeteor(GameObject possibleMeteor) {
		int i = SearchMeteor(possibleMeteor);
		if(i != -1) {
			ReturnToPool(i);
			return;
		}
		i = SearchChildMeteor(possibleMeteor); 
		if(i != -1) {
			ReturnToChildPool(i);
		}
	}

	void ReturnToPool(int i) {
		SpawnChildren(GetFirstUnusedChild(), meteors[i].meteor);

		meteors[i].is_alive = false;
		meteors[i].meteor.SetActive(false);
	}

	void ReturnToChildPool(int i) {
		childMeteors[i].is_alive = false;
		childMeteors[i].childMeteor.SetActive(false);
	}

	void UpdateMeteors() {
		for(int i = 0; i < meteors.Length; i++) {
			MeteorWrapper mt = meteors[i];
			if(mt.is_alive) {
				mt.counter -= Time.deltaTime;
				if(mt.counter <= 0) {
					ReturnToPool(i);
				}
			}
		}
		for(int i = 0; i < childMeteors.Length; i++) {
			ChildMeteorWrapper mt = childMeteors[i];
			if(mt.is_alive) {
				mt.counter -= Time.deltaTime;
				if(mt.counter <= 0) {
					ReturnToChildPool(i);
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

	int GetFirstUnusedChild() {
		for(int i = 0; i < childMeteors.Length; i++) {
			if(!childMeteors[i].is_alive) return i;
		}
		return -1;
	}

	void SpawnChildren(int n, GameObject parentMeteor, bool second = false, int dir = -1) {
		if(n != -1) {
			childMeteors[n].is_alive = true;

			childMeteors[n].childMeteor.transform.SetPositionAndRotation(parentMeteor.transform.position, Quaternion.identity);
			childMeteors[n].childMeteor.SetActive(true);
			childMeteors[n].counter = maxLife;
			RigidbodyReset(childMeteors[n].childMeteor);

			childMeteors[n].childMeteor.GetComponent<Rigidbody>().AddForce(new(dir * launchForce, 0, 0));
		}
		if(!second) {
			SpawnChildren(GetFirstUnusedChild(), parentMeteor, true, 1);
		}
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
