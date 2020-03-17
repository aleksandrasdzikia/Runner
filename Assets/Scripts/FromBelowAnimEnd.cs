using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromBelowAnimEnd : MonoBehaviour
{
	private bool destroy = false;

	void Update() {
		if (this.destroy) {
			this.transform.position = new Vector3(0, this.transform.position.y + 15f, 0);
			Destroy(this);
		}
	}

	void End() {
		Destroy(this.GetComponent<Animator>());
		this.destroy = true;
	}
}