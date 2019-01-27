using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static Manager instance;

	int cash;

	public TMPro.TextMeshProUGUI cashText;
	public UnityEngine.UI.Slider slider;

	void Start()
    {
		if (FindObjectsOfType<Manager>().Length > 1)
		{
			Destroy(gameObject);
		}    
		else
		{
			instance = this;

			AddOxygen(60);
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

    void Update()
    {
		AddOxygen(-Time.deltaTime * 0.6f);
		if (instance.slider.value <= 0)
		{
			// GG
			// YOU DEADED
		}
    }
}
