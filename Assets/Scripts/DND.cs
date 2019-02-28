using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DND : MonoBehaviour
{

	// Use this for initialization

	public Canvas can;
	void Start()
	{
		if(this.gameObject.name.Equals("GameObject"))
		{
			DontDestroyOnLoad (this);
			DontDestroyOnLoad (can);
		}
	}
}
