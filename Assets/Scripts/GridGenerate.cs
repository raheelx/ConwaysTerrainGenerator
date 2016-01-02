/*
	Generates the Grid that the player uses to generate the terrain
	Dependent on GameData.cs for the size of the grid
*/

using UnityEngine;
using System.Collections;

public class GridGenerate : MonoBehaviour 
{	

	//public variables
	public GameObject ParentGrid;	//Empty object to contain the cubes for the game board
	public GameObject Cube;	//Makes up the grid
	public float Spacing;	//Spacing between the cubes in the grid
	
	
	void Awake () 
	{
		GenerateGrid();
	}
	
	
	
	
	void GenerateGrid () 
	{
		GameData gameData = GetComponent<GameData>();
		int rows = gameData.Rows;
		int cols = gameData.Cols;
		
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				GenerateCube(i, j);
			}
		}
	}
	
	void GenerateCube (int iRow, int jCol) 
	{
		float offset = Cube.transform.localScale.z + Spacing;
		GameObject cubeTemp = Instantiate(Cube) as GameObject;
		cubeTemp.transform.SetParent(ParentGrid.transform);
		cubeTemp.transform.localPosition = Vector3.zero + new Vector3(offset * jCol, 0, offset * iRow);
	}
	
}
