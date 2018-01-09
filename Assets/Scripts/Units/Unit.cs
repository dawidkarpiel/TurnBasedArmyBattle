using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit: MonoBehaviour{

	public float hp;
	public int damage;
	public int attackRange;
	public Vector2 position;
	public int walkRange;
	public bool hasBeenAlreadySelected = false;
	public bool hasAttacked = false;
	
	GameController controller;
	public int moves;

	public Team team;

	public void DealDamage(float damage)
	{
		hp -= damage;
		if(hp <= 0)
		{
			Die();
		}
	}

	public void Die()
	{

	}

	public void Activate()
	{
		moves = walkRange;
		hasAttacked = false;
	} 

	public void Move(Vector3 position, int distance)
	{
		moves -= distance;
		StartCoroutine(moveLerp(position));
	}

	IEnumerator moveLerp(Vector3 position)
	{
		while(this.gameObject.transform.position != position)
		{
			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, position, 1);
			yield return new WaitForFixedUpdate();
		}
	}

	public bool isAlive()
	{
		if(hp <= 0)
		{
			return false;
		}
		else
			return true;

	}
}
