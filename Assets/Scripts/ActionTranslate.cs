using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ActionTranslate : MonoBehaviour, IAction
{
	public GameObject translateToObject;

	void Start() {
		this.GetComponent<Collider>().isTrigger = true;
	}

	public Vector3 getTranslateVector() {
		if (this.translateToObject == null)
			return Vector3.zero;
		return translateToObject.transform.position - this.transform.position;
	}

    public void LinkTo(GameObject go) {
		this.translateToObject = go;
	}

    void OnRenderObject()
    {
       if (FindObjectOfType<DebugArrows>().DrawArrows)
       {
           if (this.translateToObject != null)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.translateToObject.transform.position, new Color(0, 255, 255), 0.25f, 20f, 1f);
       }
       else
       {
           return;
       }
    }
}
