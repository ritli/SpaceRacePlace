using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewHandler : MonoBehaviour
{
	private MeshFilter[] previewModelFilters;
	public static PreviewHandler instance;

	void Start()
	{
		previewModelFilters = new MeshFilter[10];

		for (int i = 0; i < 10; i++)
		{
			var previewModel = new GameObject("Preview", typeof(MeshRenderer), typeof(MeshFilter)).GetComponent<MeshRenderer>();
			previewModelFilters[i] = previewModel.GetComponent<MeshFilter>();
			previewModel.transform.position = transform.position + Vector3.up * 200;
			previewModel.material = Resources.Load<Material>("PreviewMat");

			previewModel.gameObject.SetActive(false);
		}

		if (FindObjectsOfType<PreviewHandler>().Length > 1)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	public static void PreviewObject(GameObject preview)
	{
		instance.PreviewObjectInternal(preview);
	}

	private void PreviewObjectInternal(GameObject preview)
	{
		for (int i = 0; i < 10; i++)
		{
			if (preview.transform.childCount > i && preview.transform.GetChild(i).GetComponent<MeshFilter>())
			{
				var target = (preview.transform.GetChild(i));

				previewModelFilters[i].mesh = target.GetComponent<MeshFilter>().mesh;
				previewModelFilters[i].transform.position = target.transform.position;
				previewModelFilters[i].transform.localScale = target.transform.localScale + Vector3.one * 0.05f * ((Mathf.Sin(Time.time * 2) + 1));
				previewModelFilters[i].transform.localRotation = target.transform.rotation;

				previewModelFilters[i].gameObject.SetActive(true);
			}

			else
			{
				previewModelFilters[i].gameObject.SetActive(false);
			}
		}
	}

	public static void StopPreview()
	{
		for (int i = 0; i < 10; i++)
		{
			instance.previewModelFilters[i].gameObject.SetActive(false);
		}
	}
}
