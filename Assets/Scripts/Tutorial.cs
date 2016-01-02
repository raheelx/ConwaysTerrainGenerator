/*
	Controls the Tutorial UI 
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour 
{

	public GameObject tutorialText;		//Text explaining how to use the program
	public GameObject tutorialImage;	//Supporting image 
	public Sprite Spacebar;	
	public Sprite Arrows;
	public Sprite Enter;
	
	private bool usedArrows;
	private bool usedEnterOnce;
	private bool tutorial;
	private Text text;	//tutorial text
	private Image img;	//tutorial image
	private PlayerInput playerInput;	//script for Player Input (used to activate the spacebar)
	
	void Start () 
	{
		playerInput = GetComponent<PlayerInput>();
		text = tutorialText.GetComponent<Text>();
		img = tutorialImage.GetComponent<Image>();
		
		text.text = "Use Arrows to Move";
		img.sprite = Arrows;
		
		tutorial = true;
	}
	
	void Update () {
		
		if (tutorial)
		{
			if (!usedArrows && !usedEnterOnce)
			{
				if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
				{
					//Arrows have been used, switch to enter tutorial
					//Activate the Enter button in PlayerInput.cs
					playerInput.EnterActivate = true;
					text.text = "Press Enter to Activate";
					img.sprite = Enter;
					usedArrows = true;
				}
			}
			else if (usedArrows && !usedEnterOnce)
			{
				if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
				{
					//Enter has been used, switch to Enter Freeze tutorial. 
					//Activate the snapshot capabality in PlayerInput.cs
					playerInput.SnapshotActivate = true;
					text.text = "Press Enter to Freeze and reactivate the arrow keys";
					img.sprite = Enter;
					usedEnterOnce = true;
				}
			}
			else if (usedArrows && usedEnterOnce)
			{
				if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
				{
					//Enter used to freeze, switch to Space tutorial
					text.text = "Press Space to Save Terrain or use the Arrows to adjust the states";
					img.sprite = Spacebar;
					usedArrows = false;	
				}
			}
			else if (!usedArrows && usedEnterOnce)	//This does not make sense conceptually but used to reduce number of booleans
			{											//Should actually read - !usedSpacebar
				if (Input.GetKeyUp(KeyCode.Space))
				{
					//Space used. Rinse and Repeat.
					text.text = "Rinse and Repeat";
					tutorialImage.SetActive(false);
					tutorial = false;
				}
			}
		}
		else
		{
			if (Input.anyKey)
			{
				//Deactivate Tutorial
				tutorialText.SetActive(false);
			}
		}
	}
}
