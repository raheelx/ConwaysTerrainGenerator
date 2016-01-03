/*
	Enters Game/Exits Game from the Menu 
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MenuInput : MonoBehaviour 
{

	public GameObject MainScene;
	public Button EnterButton;
	
	void OnEnable () 
	{
		EnterButton.onClick.AddListener(GoToMainScene);
	}
	
	void Update () 
	{
		if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			GoToMainScene();
		}
		else if (Input.GetKeyUp(KeyCode.Escape))
			UnityEditor.EditorApplication.isPlaying = false;
	}
	
	public void GoToMainScene () 
	{
		MainScene.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
