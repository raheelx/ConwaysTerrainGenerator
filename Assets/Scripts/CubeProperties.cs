/*
	Stores the Cubes current status
	Dependent on PlayerInput.cs and GameData.cs to get updated
*/

using UnityEngine;
using System.Collections;

public class CubeProperties : MonoBehaviour 
{

	public bool Alive;
	public int NoOfLivingNeighbours;
}
