using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public List<Unit> blueTeam;
	public List<Unit> redTeam;

	public GameObject footmanPrefab;
	public GameObject golemPrefab;

	public MapController map;

	public int teamsSize;
	public Team activeTeam;

	Unit activeUnit;

	int turn;
	
	public void AddUnit(bool isRedTeam, bool isFootman)
	{
		GameObject unit;
		List<Unit> teamList;
		Team team;
		Vector2 map_position;

		if(isFootman)
			unit = footmanPrefab;
		else
			unit = golemPrefab;

		if(isRedTeam)
		{
			teamList = redTeam;
			team = Team.red;
			map_position = map.GetMapSize();
		}
		else	
		{
			teamList = blueTeam;
			team = Team.blue;
			map_position = Vector2.zero;
		}		

		Vector3 position = map.getTilePosition(new Vector2(map_position.x, teamList.Count));
		GameObject instantiatedPrefab = Instantiate(unit, position, Quaternion.identity, this.transform);

		Unit military = instantiatedPrefab.GetComponent<Unit>();
		military.team = team;
		military.position = new Vector2(map_position.x, teamList.Count);
		map.map[new Vector2(map_position.x, teamList.Count)].state = TileState.occupied;
		map.map[new Vector2(map_position.x, teamList.Count)].unitOnTile = military;


		teamList.Add(military);
		CheckAreTeamsFull();
	}

	public void CheckAreTeamsFull()
	{
		if(redTeam.Count == teamsSize && blueTeam.Count == teamsSize)
			StartGame();
	}

	public void StartGame()
	{
		turn = 0;
		activeTeam = Team.blue;
		NextTurn();
	}

	public void TileClicked(Vector2 position)
	{
		if(activeUnit != null && map.map[position].state == TileState.readyToBeOccupied)
		{
			map.map[activeUnit.position].state = TileState.free;
			map.map[activeUnit.position].unitOnTile = null;
			activeUnit.Move(map.getTilePosition(position));
			activeUnit.position = position;
			map.map[position].state = TileState.occupied;
			map.map[position].unitOnTile = activeUnit;
			map.HideFreeSpaces();
		}

		if(activeUnit != null && map.map[position].state == TileState.inAttackRange)
		{
			map.map[position].unitOnTile.DealDamage(activeUnit.damage);
			if(!map.map[position].unitOnTile.isAlive())
				UnitKilled(map.map[position].unitOnTile);
		}
	}

	public void NextTurn()
	{
		if(activeUnit != null)
			activeUnit.hasBeenAlreadySelected = true;
			
		if(activeTeam == Team.red)
		{
			activeTeam = Team.blue;
			GetFirstNotUsedUnit(blueTeam);
		}
		else
		{
			activeTeam = Team.red;
			GetFirstNotUsedUnit(redTeam);
		}

		if(activeUnit.isAlive())
		{
			Debug.Log("activate");
			activeUnit.Activate();
			map.ShowFreeSpaces(activeUnit.position);
		}
	}

	public void GetFirstNotUsedUnit(List<Unit> team)
	{
		Debug.Log("get");
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

	public void NewTurn(List<Unit> team)
	{
		foreach(Unit enter in team)
		{
			enter.hasBeenAlreadySelected = false;
		}

		GetFirstNotUsedUnit(team);
	}

	public void UnitKilled(Unit unit)
	{
		List<Unit> teamList;
		if(unit.team == Team.red)
			teamList = redTeam;
		else
			teamList = blueTeam;

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
