using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
	public MapController map;

	public GameController gameController;
	

	public void GetMap()
	{
		map.GetMap();
	}

	public void AddFootmanRed()
	{
		gameController.AddUnit(true, true);
	}
	public void AddFootmanBlue()
	{
		gameController.AddUnit(false, true);
	}

	public void NextTurn()
	{
		gameController.NextTurn();
	}

}
