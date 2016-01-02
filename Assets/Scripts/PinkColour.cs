/*
	Changes an object's colour to Pink
*/

using UnityEngine;
using System.Collections;

public class PinkColour : MonoBehaviour 
{

	void Awake () 
	{
		this.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
	}
}
