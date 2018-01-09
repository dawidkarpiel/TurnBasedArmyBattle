using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexController : MonoBehaviour
{	
	
	Vector2 hexPosition;
	public Vector3 localPosition
	{
		get
		{
			return this.gameObject.transform.position;
		}
	}

	private TileState _state = TileState.free;
	public TileState state
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
			view.RefreshView(_state);
		}
	}
	
	[HideInInspector]
	public int distanceToActiveUnit;

	MapController map;

	HexView view;

	[HideInInspector]
	public Unit unitOnTile;

	public void TileInitialize(Vector2 position, MapController map)
	{
		view = GetComponent<HexView>();
		state = TileState.free;
		this.hexPosition = position;
		this.map = map;
	}

	void OnMouseDown()
	{
		map.TileClicked(hexPosition);
	}
}

public enum TileState
{
	free = 0,
	occupied = 1,
	readyToBeOccupied = 2,
	inAttackRange = 3,
	active = 4,
}

