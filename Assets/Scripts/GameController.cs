using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameController : MonoBehaviour {

	public List<Unit> blueArmy;
	public List<Unit> redArmy;

	[SerializeField]
	GameObject footmanPrefab;
	[SerializeField]
	GameObject golemPrefab;

	public MapController map;

	Team activeTeam;

	Unit activeUnit;

	public int blueTeamFootmans = 0;
	public int blueTeamGolems = 0;

	public int redTeamFootmans = 0;
	public int redTeamGolems = 0;

	public Vector2 mapSize;

	bool areArmyPositioned = false;

	public static Action gameEnded = delegate {};

	public void InitializeGameplay()
	{
		areArmyPositioned = false;
		map.GetMap(mapSize);
		AddRedTeamUnits();
		AddBlueTeamUnits();
		StartGame();
	}
	
	void AddRedTeamUnits()
	{
		redArmy = new List<Unit>();
		AddFootmans(redArmy, redTeamFootmans);
		AddGolems(redArmy, redTeamGolems);
		SetTeam(redArmy, Team.blue);
	}

	void AddBlueTeamUnits()
	{
		blueArmy = new List<Unit>();
		AddFootmans(blueArmy, redTeamFootmans);
		AddGolems(blueArmy, redTeamGolems);
		SetTeam(blueArmy, Team.red);
	}

	void AddFootmans(List<Unit> team, int count)
	{
		for(int i =0; i< count; i++)
		{
			AddUnit(team, footmanPrefab);
		}
	}

	void AddGolems(List<Unit> team, int count)
	{
		for(int i =0; i< count; i++)
		{
			AddUnit(team, golemPrefab);
		}
	}
	
	void AddUnit(List<Unit> team, GameObject prefab)
	{
		GameObject pooledPrefab = Instantiate(prefab, new Vector3(-100, -100, -100), Quaternion.identity, this.transform);

		Unit military = pooledPrefab.GetComponent<Unit>();

		team.Add(military);
	}

	void SetTeam(List<Unit> army, Team team)
	{
		foreach(Unit soldier in army)
		{
			soldier.team = team;
		}
	}

	
	void StartGame()
	{
		activeTeam = Team.blue;
		PositionArmies();
	}
	
	void PositionArmies()
	{
		if(!areArmyPositioned)
		{
			if(activeTeam == Team.red)
			{
				activeTeam = Team.blue;
				GetFirstNotUsedUnit(blueArmy);
			}
			else
			{
				activeTeam = Team.red;
				GetFirstNotUsedUnit(redArmy);
			}

			if(activeUnit == null)
			{
				areArmyPositioned = true;
				Turn();
			}
			else
			{
				EnablePositionArmies();
			}
		}
		else
			Turn();
	}

	void EnablePositionArmies()
	{
		if(activeTeam == Team.red)
			{
				map.EnableAllRows(0);
				map.EnableAllRows(1);
			}
			else
			{
				map.EnableAllRows((int)mapSize.x - 1);
				map.EnableAllRows((int)mapSize.x - 2);
			}
	}

	public void TileClicked(Vector2 position)
	{
		var tile = map.map[position];

		if(!areArmyPositioned && tile.state == TileState.readyToBeOccupied)
		{
			PositionSoldier(position);
		}
		else if(activeUnit != null && tile.state == TileState.readyToBeOccupied)
		{
			ChangeUnitPosition(tile, position);
		}
		else if(activeUnit != null && tile.state == TileState.inAttackRange)
		{
			AttackUnit(tile);
		}
	}
	
	void ChangeUnitPosition(HexController toTile, Vector2 position)
	{
		map.map[activeUnit.position].state = TileState.free;
		map.map[activeUnit.position].unitOnTile = null;
		
		activeUnit.Move(map.getTilePosition(position), toTile.distanceToActiveUnit);
		activeUnit.position = position;

		toTile.state = TileState.occupied;
		toTile.unitOnTile = activeUnit;

		map.ShowRange(activeUnit.position);
	}

	void PositionSoldier(Vector2 position)
	{
		var tile = map.map[position];
		activeUnit.Move(map.getTilePosition(position), 0);
		activeUnit.position = position;
		activeUnit.hasBeenAlreadySelected = true;

		tile.state = TileState.occupied;
		tile.unitOnTile = activeUnit;

		map.ClearTiles();
		PositionArmies();
	}

	public void Turn()
	{
		if(!areArmyPositioned)
			return;

		if(activeUnit != null)
			activeUnit.hasBeenAlreadySelected = true;

		if(activeTeam == Team.red)
		{
			activeTeam = Team.blue;
			NextTurn(blueArmy);

		}
		else
		{
			activeTeam = Team.red;
			NextTurn(redArmy);
		}
	}
	
	void NextTurn(List<Unit> army)
	{
		GetFirstNotUsedUnit(army);

		if(activeUnit == null)
			NewTurn(army);

		if(activeUnit.isAlive())
		{
			activeUnit.Activate();
			map.ShowRange(activeUnit.position);
		}
	}

	void GetFirstNotUsedUnit(List<Unit> team)
	{
		activeUnit = null;

		foreach(Unit enter in team)
		{
			if(!enter.hasBeenAlreadySelected)
			{
				activeUnit = enter;
				break;		
			}
		}

		if(!areArmyPositioned && activeUnit == null)
		{
			return;
		}
	}

	void NewTurn(List<Unit> team)
	{
		if(!areArmyPositioned)
		{
			return;
		}

		foreach(Unit enter in team)
		{
			enter.hasBeenAlreadySelected = false;
		}

		GetFirstNotUsedUnit(team);
	}

	void AttackUnit(HexController tile)
	{
		if(!activeUnit.hasAttacked)
		{
			tile.unitOnTile.DealDamage(activeUnit.damage);
			activeUnit.hasAttacked = true;
			Debug.Log("dealt " + activeUnit.damage + " damage");
		}
		else
		{
			Debug.Log("you had already attacked");
		}


		if(!tile.unitOnTile.isAlive())
			UnitKilled(tile.unitOnTile);

		Debug.Log("attack");
	}

	void UnitKilled(Unit unit)
	{
		List<Unit> teamList;

		if(unit.team == Team.red)
			teamList = blueArmy;
		else
			teamList = redArmy;

		map.map[unit.position].unitOnTile = null;

		for(int i =0; i < teamList.Count; i++)
		{
			Debug.Log(teamList[i].position);
			
			if(teamList[i] == unit)
			{
				teamList.RemoveAt(i);
				Debug.Log("removed at " + i);
			}
		}

		Destroy(unit.gameObject);


		map.ClearTiles();

		Debug.Log("killed");

		CheckVictoryConditions();
	}	

	void CheckVictoryConditions()
	{
		if(redArmy.Count == 0 || blueArmy.Count == 0)
			EndGame();

	}

	public void EndGame()
	{
		blueArmy.Clear();
		redArmy.Clear();

		var children = new List<GameObject>();
		foreach (Transform child in transform) children.Add(child.gameObject);
		children.ForEach(child => Destroy(child));

		gameEnded();
	}
}
public enum Team
{
	blue = 0,
	red = 1,
}
