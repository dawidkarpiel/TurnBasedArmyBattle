using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexView : MonoBehaviour
{
	public Color free;
	public Color occupied;
	public Color readyToBeOccupied;
	public Color inAttackRange;
	public Color active;

	[SerializeField]
	SpriteRenderer image;

	public void RefreshView(TileState state)
	{
		switch(state)
		{
			case TileState.free:
			image.color = free;
			break;

			case TileState.occupied:
			image.color = occupied;
			break;

			case TileState.readyToBeOccupied:
			image.color = readyToBeOccupied;
			break;

			case TileState.inAttackRange:
			image.color = inAttackRange;
			break;

			case TileState.active:
			image.color = active;
			break;
		}
	}
}