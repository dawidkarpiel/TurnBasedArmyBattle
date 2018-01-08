using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

	public GameController gameController;

	public Slider teamSize;

	public Slider redTeamUnit1;

	public Slider redTeamUnit2;


	public Slider blueTeamUnit1;
	public Slider blueTeamUnit2;

	public Slider mapSize_x;
	public Slider mapSize_y;
	

	public void StartGame()
	{
		gameController.blueTeamFootmans = (int)blueTeamUnit1.value;
		gameController.blueTeamGolems = (int)blueTeamUnit2.value;

		gameController.redTeamFootmans = (int)redTeamUnit1.value;
		gameController.redTeamGolems = (int)redTeamUnit2.value;

		gameController.mapSize.x = (int)mapSize_x.value;
		gameController.mapSize.y = (int)mapSize_y.value;

		gameController.InitializeGameplay();
	}

	public void NextTurn()
	{
		gameController.NextTurn();
	}

	public void CheckMaxSliderValues()
	{
		
	}



}
