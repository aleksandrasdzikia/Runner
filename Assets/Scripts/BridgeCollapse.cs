using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeCollapse : MonoBehaviour, Collapseable
{
	private bool active = false;
	private bool falling = false;

	void Update() {
		if (this.falling)
			this.transform.Translate(-Vector3.up * Time.deltaTime);
	}

	public void activate() {
		this.active = true;
		float angle = Random.Range(-45, 45);
		foreach (Transform child in this.transform)
			child.Rotate(Vector3.up, angle);
	}

	void OnTriggerEnter(Collider collider) {
		if (!this.active)
			return;
		Debug.Log("touched");
		Player player = collider.GetComponent<Player>();
		if (player != null && !player.isInAir()) {
			ActionDeath action = new ActionDeath();
			action.type = DeathType.Gap;
			player.doAction(action);
			this.falling = true;
		}
	}
}
