using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController: MonoBehaviour
{
	public GameController controller;
	public MapGenerator generator;

	public Dictionary<Vector2, HexController> map;

	public void GetMap(Vector2 size)
	{
		map = new Dictionary<Vector2, HexController>();
		map = generator.GenerateMap(size);
 	}

	public void SetGenerator(MapGenerator generator)
	{
		this.generator = generator;
	}

	public void ShowFreeSpaces(Vector2 basePosition)
	{
		HideFreeSpaces();
		foreach(KeyValuePair<Vector2,HexController> tile in map)
		{
			TileState state = tile.Value.state;
			if(tile.Key.x == basePosition.x)
			{
				if(tile.Key.y == basePosition.y || tile.Key.y == basePosition.y -1  || tile.Key.y == basePosition.y + 1)
				{
					state = TileState.readyToBeOccupied;
				}
			}
			else if(tile.Key.x == basePosition.x - 1 || tile.Key.x == basePosition.x + 1)
			{
				if(tile.Key.x % 2 == 0)
				{
					if(tile.Key.y == basePosition.y || tile.Key.y == basePosition.y - 1)
					{
						state = TileState.readyToBeOccupied;
					}
				}
				else
				{
					if(tile.Key.y == basePosition.y || tile.Key.y == basePosition.y + 1)
					{
						state = TileState.readyToBeOccupied;
					}
				}
			}

			if(tile.Value.state == TileState.occupied && tile.Value.unitOnTile.team != map[basePosition].unitOnTile.team && state == TileState.readyToBeOccupied)
				state = TileState.inAttackRange;
			else if(tile.Value.state == TileState.occupied)
				state = TileState.occupied;

			if(tile.Key == basePosition)
				state = TileState.active;

			tile.Value.state = state;
		}
	}

	public void HideFreeSpaces()
	{
		foreach(KeyValuePair<Vector2,HexController> tile in map)
		{
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
