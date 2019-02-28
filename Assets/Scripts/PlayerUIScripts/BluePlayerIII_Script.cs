using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePlayerIII_Script : MonoBehaviour 
{
	public static string BluePlayerIII_ColName;

	void OnTriggerEnter2D(Collider2D col)
	{
	
		if (col.gameObject.tag == "blocks") 
		{
		
			BluePlayerIII_ColName = col.gameObject.name;

			if (col.gameObject.name.Contains ("Safe House")) 
			{
				
				print ("Entered PlayerI BlueI in safe house");
			
			}
		}
	}
	// Use this for initialization
	void Start () 
	{
		
		BluePlayerIII_ColName = "none";
	
	}
}
