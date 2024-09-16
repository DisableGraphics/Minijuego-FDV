using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
	public float speed = 10f;
	public float maxLifeTime = 3f;
	public Vector3 targetVector;

	private Text texto;	

    // Start is called before the first frame update
    void Start()
    {
		// Small optimization so we aren't looking for the gameObject everytime I call UpdateScoreText()
		texto = GameObject.FindGameObjectWithTag("UI").GetComponent<Text>();
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * targetVector);
    }

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.CompareTag("Enemy")) {
			IncreaseScore();
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

	private void IncreaseScore() {
		Jugador.score++;
		UpdateScoreText();
	}

	private void UpdateScoreText() {
		texto.text = "Puntos: " + Jugador.score;
	}
}
