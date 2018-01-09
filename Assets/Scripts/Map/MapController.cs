using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController: MonoBehaviour
{
	GameController controller;
	MapGenerator generator;

	public Dictionary<Vector2, HexController> map;

	void Start()
	{
		controller = GetComponent<GameController>();
		generator = GetComponent<MapGenerator>();

	}

	public void GetMap(Vector2 size)
	{
		map = new Dictionary<Vector2, HexController>();
		map = generator.GenerateMap(size);
 	}

	public void ShowRange(Vector2 basePosition)
	{
		ClearTiles();

		map[basePosition].distanceToActiveUnit = 0;
		ShowSpaces(basePosition);
	}

	void ShowSpaces(Vector2 basePosition)
	{
		var unit = map[basePosition].unitOnTile;
		int walkRange = unit.moves;
		int attackRange = unit.attackRange;

		for(int i =0; i < walkRange+attackRange; i++)
		{
			foreach(KeyValuePair<Vector2,HexController> tile in map)
			{
				if(tile.Value.distanceToActiveUnit != i)
					continue;
				
				IncreaseDistanceOnNeighbouringTiles(tile.Key);
			}
		}		
		
		
		foreach(KeyValuePair<Vector2,HexController> tile in map)
		{
			var distance = tile.Value.distanceToActiveUnit;
			if(distance == 0)
				tile.Value.state = TileState.active;
			else if(distance == -1 && tile.Value.state == TileState.occupied)
				tile.Value.state = TileState.occupied;
			else if(distance == -1)
				tile.Value.state = TileState.free;
			else if(tile.Value.state == TileState.occupied && tile.Value.unitOnTile.team != map[basePosition].unitOnTile.team && distance <= attackRange)
				tile.Value.state = TileState.inAttackRange;
			else if(tile.Value.state == TileState.occupied)
				tile.Value.state = TileState.occupied;
			else if(distance <= walkRange)
				tile.Value.state = TileState.readyToBeOccupied;
		}
	}

	void IncreaseDistanceOnNeighbouringTiles(Vector2 basePosition)
	{
		int baseDistance = map[basePosition].distanceToActiveUnit;

		foreach(KeyValuePair<Vector2,HexController> tile in map)
		{

			if(tile.Value.distanceToActiveUnit == -1)
			{	
				if(tile.Key.x == basePosition.x)
				{
					if(tile.Key.y == basePosition.y || tile.Key.y == basePosition.y -1  || tile.Key.y == basePosition.y + 1)
					{
						tile.Value.distanceToActiveUnit = baseDistance + 1;
					}
				}
				else if(tile.Key.x == basePosition.x - 1 || tile.Key.x == basePosition.x + 1)
				{
					if(tile.Key.x % 2 == 0)
					{
						if(tile.Key.y == basePosition.y || tile.Key.y == basePosition.y - 1)
						{
							tile.Value.distanceToActiveUnit = baseDistance + 1;
						}
					}
					else
					{
						if(tile.Key.y == basePosition.y || tile.Key.y == basePosition.y + 1)
						{
							tile.Value.distanceToActiveUnit = baseDistance + 1;
						}
					}
				}
			}
		}
	}

	public void ClearTiles()
	{
		foreach(KeyValuePair<Vector2,HexController> tile in map)
		{
			tile.Value.distanceToActiveUnit = -1;

			if(tile.Value.unitOnTile != null)
				tile.Value.state = TileState.occupied;
			else
				tile.Value.state = TileState.free;
		}
	}

	public void EnableAllRows(int num)
	{
		foreach(KeyValuePair<Vector2,HexController> tile in map)
		{
			if(tile.Value.state == TileState.occupied)
				continue;
			else if(tile.Key.x == num && tile.Value.state != TileState.occupied)
				tile.Value.state = TileState.readyToBeOccupied;
			else if(tile.Value.state != TileState.readyToBeOccupied)
				tile.Value.state = TileState.free;
		}
	}
	public Vector3 getTilePosition(Vector2 hexPosition)
	{
		return map[hexPosition].localPosition;
	}

	public void TileClicked(Vector2 position)
	{
		controller.TileClicked(position);
	}
}
