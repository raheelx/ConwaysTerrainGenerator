/*
	Called and Updated from PlayerInput.cs
	
	Public Functions:
		Name					Affected variables	
	
		GetProperties 			{gridHeights; alive}
		SetProperties 			{gridHeights; alive}
		AdjustCubeHeights		{gridHeights}
		ConwaysRules			{alive}
*/

using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour 
{	

	//public variables
	public int XPositionOfPlayer = 0;
	public int ZPositionOfPlayer = 0;
	public int Rows;	//Grid rows
	public int Cols;	//Grid columns
	public GameObject Grid; //The game board
	public bool[,] AliveCubes;
	public float HeightIncrements;	//The inc value that the cube will grow or shrink by when alive or dead
	public float MaxHeightOfCube;
	
	//private variables
	private float[,] gridHeights;	//The heights of each cube in the grid
	private Transform[] gridCubes;
	private CubeProperties[] cubeProperties;		//The properties of each cube in the grid
	private int[,] liveNeighbours;	//The no. of living neighbours of each cube
	
	
	
	
	void Awake () 
	{
		gridHeights = new float[Rows,Cols];
		AliveCubes = new bool[Rows,Cols];
		liveNeighbours = new int[Rows,Cols];
	}
	
	void Start () 
	{
		GetProperties();
	}
	
		
	//Get the cube's current height and status (dead or alive)
	public void GetProperties () 
	{	
		//Get the cubes into an array - gridCubes
		gridCubes = new Transform[Grid.transform.childCount];
		cubeProperties = new CubeProperties[Grid.transform.childCount];
		
		for (int i = 0; i < Grid.transform.childCount; i++)
		{
			gridCubes[i] = Grid.transform.GetChild(i).transform;
			cubeProperties[i] = Grid.transform.GetChild(i).GetComponent<CubeProperties>();
		}
		
		int k = 0;
		
		//Stores the heights, alive status, and number of alive neighbours
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Cols; j++)
			{
				gridHeights[i,j] = gridCubes[k].localScale.y;
				AliveCubes[i,j] = cubeProperties[k].Alive;
				liveNeighbours[i,j] = cubeProperties[k].NoOfLivingNeighbours;
				k++;
			}
		}
			
	}
	
	//Sets the cube's height, alive status, and numnber of living neighbours
	public void SetProperties (float startTime, float currentTime, float lerpTime) 
	{
		int k = 0;
		
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Cols; j++)
			{
				//Scale the cubes - Grows in both directions
				Vector3 cubeScale = Grid.transform.GetChild(k).transform.localScale;
				cubeScale.y = gridHeights[i,j];
				Grid.transform.GetChild(k).transform.localScale = Vector3.Lerp(Grid.transform.GetChild(k).transform.localScale, cubeScale,(currentTime - startTime)/lerpTime);
				
				//Grow upwards only - scale : position is 2:1
				Vector3 cubePosition = Grid.transform.GetChild(k).transform.localPosition;
				cubePosition.y = gridHeights[i,j]/2;
				Grid.transform.GetChild(k).transform.localPosition = Vector3.Lerp(Grid.transform.GetChild(k).localPosition, cubePosition, (currentTime - startTime)/lerpTime);
				
				Grid.transform.GetChild(k).GetComponent<CubeProperties>().Alive = AliveCubes[i,j];
				Grid.transform.GetChild(k).GetComponent<CubeProperties>().NoOfLivingNeighbours = liveNeighbours[i,j];
				k++;
			}
		}
	}
	
	
	//If the cubes are alive they grow else they shrink
	public void AdjustCubeHeights () 
	{	
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Cols; j++)
			{
				if (AliveCubes[i,j] && gridHeights[i,j] < MaxHeightOfCube)
					gridHeights[i,j] += HeightIncrements;
				else if (!AliveCubes[i,j] && gridHeights[i,j] > 0)
					gridHeights[i,j] -= HeightIncrements;
			}
		}
	}
	
	
	
	public void ConwaysRules () 
	{
		GetLiveNeighbours();
		
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Cols; j++)
			{	
				/*
					Rule 1: if < 2 neighbours are alive, you die.
					Rule 2: if > 3 neighbours are alive, you die.
					Rule 3: if live neighbours == 2 or 3, you stay alive.
					Rule 4: if you're dead and you have 3 living neighbours, you come to life
				*/
				if (AliveCubes[i,j])
				{
					if (liveNeighbours[i,j] < 2 || liveNeighbours[i,j] > 3)
						AliveCubes[i,j] = false;
				} 
				else
				{
					if (liveNeighbours[i,j] == 3)
						AliveCubes[i,j] = true;
				}
			}
		}
	}
	
	//Stores the number of alive neighbours of each block in array - liveNeighbours
	void GetLiveNeighbours () 
	{	
		for (int i = 0; i < Rows; i++)
		{
			for (int j = 0; j < Cols; j++)
			{
				int count = 0;
					
				if (CheckNeighbour(i-1, j))	//Top
					count++;
					
				if (CheckNeighbour(i+1, j))	//Bottom
					count++;				
					
				if (CheckNeighbour(i, j+1))	//Right
					count++;
					
				if (CheckNeighbour(i, j-1))	//Left
					count++;
					
				if (CheckNeighbour(i-1, j+1))	//Top-right
					count++;
					
				if (CheckNeighbour(i-1, j-1))	//Top-left
					count++;
					
				if (CheckNeighbour(i+1, j+1))	//Bottom-right
					count++;
					
				if (CheckNeighbour(i+1, j-1))	//Bottom-left
					count++;
				
				liveNeighbours[i,j] = count;
				
			}
		}
	}
	
	//Returns true if the input block is alive
	bool CheckNeighbour(int r, int c) 
	{
		if ((r < Rows && r > -1) && (c < Cols && c > -1))
		{
			if (AliveCubes[r,c])
				return true;
			else return false;
		}
		else return false;
	}
	

	
	
}
