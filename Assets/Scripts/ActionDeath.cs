using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ActionDeath : MonoBehaviour, IAction
{
	public DeathType type = DeathType.Instant;

	void Start() {
		this.GetComponent<Collider>().isTrigger = true;
	}

	public DeathType getType() {
		return this.type;
	}
}