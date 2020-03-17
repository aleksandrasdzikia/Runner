using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGold : MonoBehaviour, IPickUp {
	public Vector2 goldRange;

	private Animator animator;

	void Start() {
		this.animator = this.GetComponent<Animator>();
	}

	public bool execute(Player player) {
		if (animator)
			this.animator.SetTrigger("PickUp");
		Debug.Log(Random.Range(this.goldRange.x, this.goldRange.y));
		return true;
	}
}