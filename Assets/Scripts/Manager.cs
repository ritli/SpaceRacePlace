using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static Manager instance;
	private Player player;
	int cash;

	public float playtime;
	public int trashCount;
	public GameObject loseCamera;

	public TMPro.TextMeshProUGUI loseText;
	public TMPro.TextMeshProUGUI cashText;
	public UnityEngine.UI.Slider slider;
	private bool gameover;

	void Start()
    {
		loseCamera.SetActive(false);
		if (FindObjectsOfType<Manager>().Length > 1)
		{
			Destroy(gameObject);
		}    
		else
		{
			instance = this;
			player = FindObjectOfType<Player>();
			loseText.enabled = false;
			AddOxygen(80);
			UpdateCash(0);
		}
    }

	public static void AddOxygen(float amount)
	{
		instance.slider.value += amount;
	}

	public static bool UpdateCash(int amount)
	{
		if (instance.cash + amount > 0)
		{
			instance.cash += amount;
			instance.cashText.text = "Cash: " + instance.cash;

			return true;
		}
		else
		{
			return false;
		}
	}

	void GameOver()
	{
		Destroy(player.gameObject);
		loseCamera.SetActive(true);

		loseText.gameObject.SetActive(true);
		loseText.enabled = true;
		print("Lose");

		loseText.text = "You trashed " + trashCount + " trashes\nSurvived for " + Mathf.CeilToInt(playtime).ToString() + " seconds\npress F12 to reset";
		gameover = true;
	}

    void Update()
    {
		AddOxygen(-Time.deltaTime * 0.6f);

		playtime += Time.deltaTime;

		if (instance.slider.value <= 0 && !gameover)
		{
			GameOver();
		}

		if (Input.GetKeyDown(KeyCode.F12))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
		}
    }
}
