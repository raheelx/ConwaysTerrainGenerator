/*
	Updates the Colours of the Cubes (Black/White = Dead/Alive)
	Dependent On: CubeProperties.cs to know whether block is alive or not
*/

using UnityEngine;
using System.Collections;

public class CubeColour : MonoBehaviour 
{

	private CubeProperties cubeProperties;
	public float AlphaLevelBlack;
	public float AlphaLevelWhite;
	
	void Start () 
	{
		cubeProperties = this.transform.parent.GetComponent<CubeProperties>();
	}
	
	void Update () 
	{
		ChangeColour(cubeProperties.Alive);
	}
	
	void ChangeColour (bool alive)
	{
		if (alive)
			this.gameObject.GetComponent<Renderer>().material.color = new Color (1,1,1,AlphaLevelWhite);
		else 
			this.gameObject.GetComponent<Renderer>().material.color = new Color (0,0,0,AlphaLevelBlack);
	}
	
	
}
