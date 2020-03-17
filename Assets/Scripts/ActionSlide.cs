using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ActionSlide : MonoBehaviour, IAction
{
	public bool slideStart = true;
	public List<GameObject> slideEnd = new List<GameObject>();

	void Start() {
		this.GetComponent<Collider>().isTrigger = true;
	}

    void OnRenderObject()
    {
        if (this.slideStart)
            foreach (GameObject obj in this.slideEnd)
                if (obj)
                    DrawArrow.ForDebugTwoPoints(this.transform.position, obj.transform.position, new Color(1f, 0.5f, 0f), 0.25f, 20f, 1f);
    }
}