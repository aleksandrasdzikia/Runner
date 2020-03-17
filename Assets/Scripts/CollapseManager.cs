using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseManager : MonoBehaviour
{
	public List<GameObject> pairs;
	public Vector2 activateCountRange;

	private int activateCount = 0;

	// Use this for initialization
	void Start() {
		this.activateCount = (int) Random.Range(this.activateCountRange.x, this.activateCountRange.y);
		while (this.activateCount > 0 && this.pairs.Count > 0) {
			int randomIndex = (int) Random.Range(0, this.pairs.Count);
			Collapseable collapseable = this.pairs[randomIndex].GetComponent<Collapseable>();
			this.pairs.RemoveAt(randomIndex);
			this.activateCount--;
			if (collapseable != null)
				collapseable.activate();
		}
	}
}
