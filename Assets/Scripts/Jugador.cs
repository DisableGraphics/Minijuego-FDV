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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
			GameObject bullet = Instantiate(bulletPrefab, gun.transform.position, Quaternion.identity);
			Bullet bulletScript = bullet.GetComponent<Bullet>();
			bulletScript.targetVector = transform.right;
		}
    }
	// Usar OnTriggerEnter arregla un bug cuando la nave se teletransporta
	void OnTriggerEnter(Collider collision) {
		if(collision.gameObject.CompareTag("Enemy")) {
			score = 0;
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		
	}
}
