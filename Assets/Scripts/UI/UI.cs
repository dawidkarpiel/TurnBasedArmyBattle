using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

	public GameController gameController;

	public Slider redTeamUnit1;

	public Slider redTeamUnit2;


	public Slider blueTeamUnit1;
	public Slider blueTeamUnit2;

	public Slider mapSize_x;
	public Slider mapSize_y;

	public GameObject settingsPanel;
	public GameObject gameplayPanel;
	public GameObject gameEndPanel;
	
	
	void Start()
	{
		settingsPanel.SetActive(true);
		gameplayPanel.SetActive(false);
		gameEndPanel.SetActive(false);

		GameController.gameEnded += GameEnd;
	}

	void OnDestroy()
	{
		GameController.gameEnded -= GameEnd;
	}

	public void StartGame()
	{
		gameController.blueTeamFootmans = (int)blueTeamUnit1.value;
		gameController.blueTeamGolems = (int)blueTeamUnit2.value;

		gameController.redTeamFootmans = (int)redTeamUnit1.value;
		gameController.redTeamGolems = (int)redTeamUnit2.value;

		gameController.mapSize.x = (int)mapSize_x.value;
		gameController.mapSize.y = (int)mapSize_y.value;

		gameController.InitializeGameplay();

		settingsPanel.SetActive(false);
		gameplayPanel.SetActive(true);
		gameEndPanel.SetActive(false);
	}

	public void NextTurn()
	{
		gameController.Turn();
	}

	void GameEnd()
	{
		gameEndPanel.SetActive(true);
		gameplayPanel.SetActive(false);
	}

	public void RestartGame()
	{
		gameEndPanel.SetActive(false);
		settingsPanel.SetActive(true);
	}
}
