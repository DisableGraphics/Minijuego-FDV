using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
	public Text text;
	bool is_paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void OnPauseClicked() {
		if(is_paused) {
			Time.timeScale = 1;
			text.text = "Pausa (Esc)";
			is_paused = false;
		} else {
			Time.timeScale = 0;
			text.text = "Resumir (Esc)";
			is_paused = true;
		}
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
			OnPauseClicked();
    }
}
