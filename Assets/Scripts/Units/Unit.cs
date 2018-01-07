using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit: MonoBehaviour{

	public float hp;
	public int damage;
	public Vector2 position;
	public int possibleMoves;
	public bool hasBeenAlreadySelected = false;
	public bool hasMoved
	{
		get
		{
			if(moves >= possibleMoves)
				return true;
			else
				return false;
		}
	}
	GameController controller;
	int moves;

	public Team team;

	public void DealDamage(float damage)
	{
		Debug.Log("damage deal " + damage);
		hp -= damage;
		if(hp <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		Debug.Log("die");
	}

	public void Activate()
	{
		moves = 0;
	} 

	public void Move(Vector3 position)
	{
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
