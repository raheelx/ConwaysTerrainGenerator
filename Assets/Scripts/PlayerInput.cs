/*
	Dependent on Tutorial.cs (For Snapshot)
	Controls the Player's Input:
		- Movement
		- Stop/Go
		- Snapshot
		- Escape
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class PlayerInput : MonoBehaviour 
{

	//public variables
	public GameObject Player;
	public GameObject Grid;		//The game board
	public float GrowTime; 		//Time for the cube to grow each cycle
	public bool StartRunning;	//activates conway's game of life on Enter
	public GameObject Audio; 	//Used for the Camera Sound when Saving
	public GameObject FullViewCamera;	//Used when taking a snapshot of the terrain
	public bool SnapshotActivate;	//Snapshot capabilities is only activated when tutorial is complete
	public bool EnterActivate;	//Activate the Enter button
	public GameObject SavedAsUIPanel;	//UI element displaying where the prefab is saved
	public GameObject SavedAsUIText;
	public float DisplaySavedAsUITimer;	//Amount of time the message will stay on screen
	
	//private variables
	private int x = 0;			//x position of the player
	private int z = 0;			//z position of the player
	private int rows;			//grid rows
	private int cols;			//grid columns
	private GameData gameData;	//contains the system's data
	private Vector3[,] cubePositions;	
	private CubeProperties[,] cubeProperties;
	private float startTime; 	//Time when player presses Enter
	private AudioSource cameraSound;
	private Camera fullViewCamera;
	private Text savedAsText;
	private	float tempTimer;	//Used to time the saved as message
	
	
	
	void Start () 
	{
		gameData = GetComponent<GameData>();
		rows = gameData.Rows;
		cols = gameData.Cols;
		cameraSound = Audio.GetComponent<AudioSource>();
		fullViewCamera = FullViewCamera.GetComponent<Camera>();
		savedAsText = SavedAsUIText.GetComponent<Text>();
		
		//Get the positions of each cube to control the player's movement
		cubePositions = new Vector3[rows,cols];
		cubeProperties = new CubeProperties[rows,cols];
		GetCubeProperties();
	}

	
	void Update () 
	{	
		if (StartRunning)
		{
			RunningSequence ();
			
			//Pauses the growing/shrinking of the cubes
			if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
			{
				StartRunning = false;
				Player.SetActive(true);
			}
		}
		//Adjusts the player's position and updates the Game data 
		else
		{	
			MovementArrowKeys(); 
			
			if (SnapshotActivate)
				SaveTerrainSpaceBar();
					
			//Start Executing Conway's Game of Life on Enter
			if (EnterActivate)
			{
				if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
				{
					Player.SetActive(false);
					gameData.ConwaysRules();
					startTime = Time.time;		//Needed for Lerping the growing/shrinking of cubes
					StartRunning = true;
				}
			}
		}

	}
	
	
	//Stores each of the cubes' world position to aid player movement
	void GetCubeProperties () 
	{
		int k = 0;
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				cubePositions[i,j] = Grid.transform.GetChild(k).transform.position;
				cubeProperties[i,j] = Grid.transform.GetChild(k).GetComponent<CubeProperties>();
				k++;
			}
		}
	}
	
	//Updates the player's x and z positions and toggles "alive" status
	void UpdateGameData () 
	{
		gameData.XPositionOfPlayer = x;
		gameData.ZPositionOfPlayer = z;
		gameData.AliveCubes[gameData.XPositionOfPlayer, gameData.ZPositionOfPlayer] = 
			!gameData.AliveCubes[gameData.XPositionOfPlayer, gameData.ZPositionOfPlayer];	
		cubeProperties[x,z].Alive = !cubeProperties[x,z].Alive;
	}
	
	//Counts the number of files in a directory - used to create a unique name for saved terrains
	int FilesCount (string folderPath)
	{
		DirectoryInfo directory = new DirectoryInfo(folderPath);
		FileInfo[] files = directory.GetFiles("*.prefab");
		int noOfFiles = 0;
		foreach(FileInfo f in files)
		{
			noOfFiles++;
		}
		return noOfFiles;
	}
	
	
	//Conway's Rules Implementation on the Terrain
	void RunningSequence () 
	{
		if (Time.time - startTime > GrowTime)
		{
			gameData.ConwaysRules();
			startTime = Time.time;
		}
		
		gameData.AdjustCubeHeights();
		gameData.SetProperties(startTime, Time.time, GrowTime);
		gameData.GetProperties();
	}
	
	
	
	//Snapshot - Saves the current state of the terrain in an empty prefab
	//This should only be activated when enter has been used at least once
	void SaveTerrainSpaceBar () 
	{
		
		//Changes view to a full view while player hold down Space key
		if (Input.GetKey(KeyCode.Space))
		{
			fullViewCamera.enabled = true;
			//Hide the Player capsule
			Player.SetActive(false);
		}
		else 
			fullViewCamera.enabled = false;	
	
		//Saves and plays a camera sound when player lets go of Space key
		if (Input.GetKeyUp(KeyCode.Space))
		{
			//Show the player capsule again 
			Player.SetActive(true);
			
			//Sound Effect
			cameraSound.Play();
			
			//Save Terrain Prefab
			int fileNumber = FilesCount("Assets/GeneratedTerrain");	//Used to create a unique file name
			UnityEditor.PrefabUtility.CreatePrefab("Assets/GeneratedTerrain/terrain(" + fileNumber + ").prefab", Grid.gameObject, UnityEditor.ReplacePrefabOptions.ReplaceNameBased);
		
			//Show the user where it's saved
			tempTimer = DisplaySavedAsUITimer;
			SavedAsUIPanel.SetActive(true);
			savedAsText.text = "Saved : Assets/GeneratedTerrain/terrain(" + fileNumber + ").prefab";
		}
		
		//Turn off the saved as message after a while
		if (SavedAsUIPanel.activeSelf)
		{
			tempTimer -= Time.deltaTime;
			
			if (tempTimer <= 0)
				SavedAsUIPanel.SetActive(false);
		}
		
		
	}
	
	
	
	//Moves player around the game board/grid and updates the Game's data
	void MovementArrowKeys ()
	{
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			if (x + 1 < rows)
			{
				x++;
				Player.transform.position = new Vector3(cubePositions[x,z].x, Player.transform.position.y, cubePositions[x,z].z);
				UpdateGameData();
			}
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			if (z - 1 > -1)
			{
				z--;
				Player.transform.position = new Vector3(cubePositions[x,z].x, Player.transform.position.y, cubePositions[x,z].z);
				UpdateGameData();
			}
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			if (x - 1 > -1)
			{
				x--;
				Player.transform.position = new Vector3(cubePositions[x,z].x, Player.transform.position.y, cubePositions[x,z].z);
				UpdateGameData();
			}			
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			if (z + 1 < rows)
			{
				z++;
				Player.transform.position = new Vector3(cubePositions[x,z].x, Player.transform.position.y, cubePositions[x,z].z);
				UpdateGameData();
			}
		}
	}
	
}
