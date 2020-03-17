using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorObject : MonoBehaviour {

	public Renderer renderer;

	public List<Material> materials = new List<Material>();
	private Hashtable mats = new Hashtable();


	void Start() {
		foreach (Material mat in this.renderer.materials) {
			foreach (Material material in this.materials) {
				Debug.Log(mat.name + " =?= " + material.name);
				if (mat.name == material.name + " (Instance)") {
					this.mats.Add(material, mat);
					Debug.Log("Adeded material");
					break;
				}
			}
		}
	}

	void Update () {
		float dayTime = DayCycle.dayTime;
		foreach (Material original in this.mats.Keys) {
			Material current = (Material) this.mats[original];
			Color initColor = original.GetColor("_Color");
			if (dayTime > 0)
				current.SetColor("_Color", Color.Lerp(initColor, Color.yellow, dayTime));
			else current.SetColor("_Color", Color.Lerp(initColor, Color.blue, Mathf.Abs(dayTime)));
		}
	}
}
