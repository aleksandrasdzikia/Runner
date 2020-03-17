using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

	public Material material;
	public List<Vector2> ranges = new List<Vector2>();
	public List<Color> colors = new List<Color>();

	private Color initColor;

	void Start() {
		if (this.material == null)
			return;
		this.initColor = material.GetColor("_Color");
	}

	void Update () {
		if (this.material == null)
			return;


		for (int i = 0; i < this.ranges.Count; i++) {
			Vector2 range = this.ranges[i];
			int colorIndex = i * 2;
			Color from = this.colors[colorIndex];
			Color to = this.colors[colorIndex + 1];
			if (range.x > range.y) {
				if (DayCycle.dayEnd || DayCycle.nightStart) {
					if (range.x >= DayCycle.dayTime && DayCycle.dayTime >= range.y) {
						float time = Mathf.Abs(range.x - DayCycle.dayTime) / (range.x - range.y);
						material.SetColor("_Color", Color.Lerp(from, to, time));
					}
				}
			} else {
				if (DayCycle.dayStart || DayCycle.nightEnd) {
					if (range.y >= DayCycle.dayTime && DayCycle.dayTime >= range.x) {
						float time = Mathf.Abs(range.x - DayCycle.dayTime) / (range.y - range.x);
						material.SetColor("_Color", Color.Lerp(from, to, time));
					}
				}
			}

		}
	}

	void OnDestroy() {
		if (this.material == null)
			return;
		material.SetColor("_Color", this.initColor);
	}
}
