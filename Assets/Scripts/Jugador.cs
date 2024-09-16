using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
	public float thrustForce = 5f, rotationSpeed = 10f;

	public GameObject gun, bulletPrefab;
	private Rigidbody rb;
	public static int score = 0;
	public float borderLimitX, borderLimitY;

	class BulletWrapper {
		public GameObject bullet;
		public bool is_alive = false;
		public float counter = 0.0f;
	};
	// Pool
	private BulletWrapper[] bullets = new BulletWrapper[70];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
		for(int i = 0; i < bullets.Length; i++) {
			bullets[i] = new();
			bullets[i].bullet = Instantiate(bulletPrefab, new(0,0), Quaternion.identity);
			bullets[i].bullet.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newPos = transform.position;
		if(newPos.x > borderLimitX)
			newPos.x = -borderLimitX + 1;
		else if(newPos.x < -borderLimitX) 
			newPos.x = borderLimitX - 1;
		else if(newPos.y > borderLimitY)
			newPos.y = -borderLimitY + 1;
		else if(newPos.y < -borderLimitY)
			newPos.y = borderLimitY - 1;
		transform.position = newPos;

		float rotation = Input.GetAxis("Rotate") * Time.deltaTime;
        float thrust = Input.GetAxis("Thrust") * Time.deltaTime;
		Vector3 thrustDirection = transform.right;
		rb.AddForce(thrust * thrustForce * thrustDirection);

		transform.Rotate(Vector3.forward, -rotation * rotationSpeed);

		if(Input.GetKeyDown(KeyCode.Space)) {
			SpawnBullet();
		}
		UpdateBullets();
    }
	// OnTriggerEnter solves a bug that happens when the spaceship teleports to the other end of the map
	void OnTriggerEnter(Collider collision) {
		if(collision.gameObject.CompareTag("Enemy")) {
			score = 0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
	// Returns first unused bullet in pool
	int GetFirstUnusedBullet() {
		for(int i = 0; i < bullets.Length; i++) {
			if(!bullets[i].is_alive) return i;
		}
		return -1;
	}

	// Check if the parameter bullet is in the bullet pool
	int Search(GameObject bullet) {
		for(int i = 0; i < bullets.Length; i++) {
			if(bullets[i].bullet == bullet) return i;
		}
		return -1;
	}

	public void RemoveBullet(GameObject possibleBullet) {
		int pos = Search(possibleBullet);
		if(pos != -1) {
			ReturnToPool(pos);
		}
	}

	private void ReturnToPool(int n) {
		bullets[n].is_alive = false;
		bullets[n].bullet.SetActive(false);
	}

	void SpawnBullet() {
		BulletWrapper bw = bullets[GetFirstUnusedBullet()];
		Bullet bulletScript = bw.bullet.GetComponent<Bullet>();
		
		bw.bullet.transform.SetPositionAndRotation(gun.transform.position, Quaternion.identity);
		bulletScript.targetVector = transform.right;
		bw.is_alive = true;
		bw.bullet.SetActive(true);
		bw.counter = Bullet.maxLifeTime;
	}

	void UpdateBullets() {
		for(int i = 0; i < bullets.Length; i++) {
			BulletWrapper bw = bullets[i];
			if(bw.is_alive) {
				bw.counter -= Time.deltaTime;
				if(bw.counter <= 0) {
					ReturnToPool(i);
				}
			}
		}
	}
}
