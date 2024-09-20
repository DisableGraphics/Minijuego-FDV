using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
	public EnemySpawner es;
	public Jugador j;
	public float speed = 10f;
	public static float maxLifeTime = 3f;
	public Vector3 targetVector;

	private Text texto;	

    // Start is called before the first frame update
    void Start()
    {
		es = FindFirstObjectByType<EnemySpawner>();
		j = FindFirstObjectByType<Jugador>();
		// Small optimization so we aren't looking for the gameObject everytime I call UpdateScoreText()
		texto = GameObject.FindGameObjectWithTag("UI").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime * targetVector);
    }

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.CompareTag("Enemy")) {
			IncreaseScore();
			Debug.Log(collision.gameObject.name);
			es.RemoveMeteor(collision.gameObject);
			j.RemoveBullet(gameObject);
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
