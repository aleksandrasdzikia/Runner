using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTriggerOld : MonoBehaviour
{
	public string actionType;
	public GameObject goToObject;

	public ActionOld getAction() {
		return null;
	}

	public string getActionType() {
		return actionType;
	}

	public Vector3 getGoToTranslate() {
		if (this.goToObject == null)
			return Vector3.zero;

		return this.goToObject.transform.position - this.transform.position;
	}
}
