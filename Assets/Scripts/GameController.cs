using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	List<Unit> blueTeam;
	List<Unit> redTeam;

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

	public void InitializeGameplay()
	{
		// map.GetMap(mapSize);
		// for(int i = 0; i <= blueTeamFootmans; i++)
		// {
		// 	InitializeUnits(footmanPrefab, Team.blue);
		// }
	}

	public void InitializeUnits(GameObject unit, Team team)
	{
		// Vector2 map_position;

		// if(team == Team.red)
		// {
		// 	map_position = mapSize;
		// }
		// else	
		// {
		// 	map_position = Vector2.zero;
		// }

		// Vector3 position = map.getTilePosition(new Vector2(map_position.x, teamList.Count));
		// GameObject instantiatedPrefab = Instantiate(unit, position, Quaternion.identity, this.transform);
		// Unit military = instantiatedPrefab.GetComponent<Unit>();
		// military.team = team;

		// military.position = new Vector2(map_position.x, teamList.Count);
		// map.map[new Vector2(map_position.x, teamList.Count)].state = TileState.occupied;
		// map.map[new Vector2(map_position.x, teamList.Count)].unitOnTile = military;

		// return military; 
	}
	
	public void AddUnit(bool isRedTeam, bool isFootman)
	{
	// 	GameObject unit;
	// 	List<Unit> teamList;
	// 	Team team;
	// 	Vector2 map_position;		

	// 	Vector3 position = map.getTilePosition(new Vector2(map_position.x, teamList.Count));
	// 	GameObject instantiatedPrefab = Instantiate(unit, position, Quaternion.identity, this.transform);

	// 	Unit military = instantiatedPrefab.GetComponent<Unit>();
	// 	military.team = team;
	// 	military.position = new Vector2(map_position.x, teamList.Count);
	// 	map.map[new Vector2(map_position.x, teamList.Count)].state = TileState.occupied;
	// 	map.map[new Vector2(map_position.x, teamList.Count)].unitOnTile = military;


	// 	teamList.Add(military);
	}

	
	public void StartGame()
	{
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
