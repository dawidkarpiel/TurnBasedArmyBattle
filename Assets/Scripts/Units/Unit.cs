using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit: MonoBehaviour
{

	[HideInInspector]
	public float hp;
	[HideInInspector]
	public int damage;
	[HideInInspector]
	public int attackRange;
	[HideInInspector]
	public Vector2 position;
	[HideInInspector]
	public int walkRange;
	[HideInInspector]
	public bool hasBeenAlreadySelected = false;
	[HideInInspector]
	public bool hasAttacked = false;
	
	[HideInInspector]
	public int moves;
	[HideInInspector]
	public Team team;
	Animator animator;
	Vector3 lookDirection;

	void Start()
	{
		animator = GetComponent<Animator>();
		if(team == Team.red)
			lookDirection = Vector3.down;
		else
			lookDirection = Vector3.up;

		transform.rotation = Quaternion.Euler(lookDirection * 90);
	}

	void FixedUpdate()
	{
	}

	public void GetHit(float damage)
	{
		hp -= damage;
		if(hp <= 0)
		{
			Die();
		}
		else
			LaunchAnimation("GetHit");
	}

	public void Attack()
	{
		hasAttacked = true;
		LaunchAnimation("Attack");
	}

	public void Die()
	{
		LaunchAnimation("Death");
		StartCoroutine(waitWithDestroy());
	}

	IEnumerator waitWithDestroy()
	{
		while(true)
		{
			yield return new WaitForSeconds(1f);
			Destroy(this.gameObject);
		}
	}

	public void Activate()
	{
		moves = walkRange;
		hasAttacked = false;
	} 

	public void Move(Vector3 position, int distance)
	{
		LaunchAnimation("Move");
		moves -= distance;
		StartCoroutine(moveLerp(position));
	}

	IEnumerator moveLerp(Vector3 position)
	{
		while(this.gameObject.transform.position != position)
		{
			this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, position, Time.deltaTime);
			yield return new WaitForFixedUpdate();
		}
	}

	public void Victory()
	{
		LaunchAnimation("Victory");
	}

	void LaunchAnimation(string name)
	{
		animator.SetTrigger(name);
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
