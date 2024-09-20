using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
	public Button resetButton, continueButton;
	bool is_paused = false;
    // Start is called before the first frame update
    void Start()
    {
        SetEnabledPauseMenu(false);
    }

	void SetEnabledPauseMenu(bool enabled) {
		resetButton.gameObject.SetActive(enabled);
		continueButton.gameObject.SetActive(enabled);
	}

	public void OnContinueClicked() {
		State(true);
	}

	public void OnResetClicked() {
		State(true);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void State(bool is_pause) {
		if(is_pause) {
			Time.timeScale = 1;
			is_paused = false;
			SetEnabledPauseMenu(false);
		} else {
			Time.timeScale = 0;
			is_paused = true;
			SetEnabledPauseMenu(true);
		}
	}

	public void OnPauseClicked() {
		State(is_paused);
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
			OnPauseClicked();
    }
}
