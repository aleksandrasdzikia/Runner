using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public static float MAX_LEVEL_HEIGHT  = 4f;

	public bool stepByStep = false;
	private bool stepping = false;

	void Start () {

	}

	void Update () {
		if (!this.stepping && (!this.stepByStep || Input.GetKeyDown("Space")))
			this.Step();
	}

	void Step() {

	}
}
