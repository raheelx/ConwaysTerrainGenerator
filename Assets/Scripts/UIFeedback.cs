/*
	Dependent on PlayerInput.cs to know when conway's rules are running
	Exits Game on Escape 
	Gives Feedback while conway's rules are being executed
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIFeedback : MonoBehaviour 
{
	
	//public variables
	public GameObject Indicator;	//UI image that indicates that the game is executing Conway's Rules
	public float Timer;	//Blinking Timer
	
	//private variables
	private PlayerInput playerInput;
	private float timer;	//local blinking timer (gets adjusted and reset to the public Timer value)

	
	void Start () 
	{
		playerInput = GetComponent<PlayerInput>();
		timer = Timer;
	}
	
	void Update () 
	{
		//Escape = Exit Game 
		if (Input.GetKeyUp(KeyCode.Escape))
			UnityEditor.EditorApplication.isPlaying = false;
			
		//Program is Running Feedback - Image blinks while program is running
		if (playerInput.StartRunning)
		{
			timer -= Time.deltaTime;
			
			if (timer <= 0)
			{
				timer = Timer;
				Indicator.SetActive(!Indicator.activeSelf);
			}
		}
		else
			Indicator.SetActive(true);
			
	}
}
