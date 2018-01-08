using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public float x_dif;
	public float y_dif;	
	public GameObject hex;

	public MapController controller;
	
	
	public Dictionary<Vector2, HexController> GenerateMap(Vector2 size)
	{
		Dictionary<Vector2, HexController> map = new Dictionary<Vector2, HexController>();

		for(int i = 0; i < size.x; i++)
		{
			for(int j = 0; j < size.y; j++)
			{
				Vector3 position = new Vector3();

				position.z = j * y_dif;
				position.x = i * x_dif;

				if(i % 2 == 0)
				{
					position.z += y_dif /2;
				}
					
				
				GameObject instantiatedTile = Instantiate(hex, position, Quaternion.identity, this.transform);
				HexController hexController = instantiatedTile.GetComponent<HexController>();

				Vector2 pos2d = new Vector2(i,j);
				hexController.TileInitialize(pos2d, controller);
				map.Add(pos2d, hexController);
			}

		}
		return map;
	}
}
