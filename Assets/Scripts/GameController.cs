using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	List<Unit> blueArmy;
	List<Unit> redArmy;

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

	public void InitializeGameplay()
	{
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
		SetTeam(blueArmy, Team.blue);
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
	}

	public void TileClicked(Vector2 position)
	{
		if(!areArmyPositioned)
		{
			PositionSoldier(position);
		}

		var tile = map.map[position];
		if(activeUnit != null && tile.state == TileState.readyToBeOccupied)
		{
			map.map[activeUnit.position].state = TileState.free;
			map.map[activeUnit.position].unitOnTile = null;
			
			activeUnit.Move(map.getTilePosition(position));
			activeUnit.position = position;

			tile.state = TileState.occupied;
			tile.unitOnTile = activeUnit;

			map.HideFreeSpaces();
		}

		if(activeUnit != null && tile.state == TileState.inAttackRange)
		{
			tile.unitOnTile.DealDamage(activeUnit.damage);
			if(!tile.unitOnTile.isAlive())
				UnitKilled(tile.unitOnTile);
		}
	}

	void PositionSoldier(Vector2 position)
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
			return;
		}

		var tile = map.map[position];
		activeUnit.Move(map.getTilePosition(position));
		activeUnit.position = position;
		activeUnit.hasBeenAlreadySelected = true;

		tile.state = TileState.occupied;
		tile.unitOnTile = activeUnit;

		map.HideFreeSpaces();
	}

	public void Turn()
	{
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

		if(activeUnit.isAlive())
		{
			activeUnit.Activate();
			map.ShowFreeSpaces(activeUnit.position);
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

		if(activeUnit == null)
			NewTurn(team);

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

	void UnitKilled(Unit unit)
	{
		List<Unit> teamList;
		if(unit.team == Team.red)
			teamList = redArmy;
		else
			teamList = blueArmy;

		teamList.Remove(unit);
		Destroy(unit.gameObject);
		map.HideFreeSpaces();
	}	
}
public enum Team
{
	blue = 0,
	red = 1,
}
