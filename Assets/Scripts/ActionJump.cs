using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ActionJump : MonoBehaviour, IAction
{
	public GameObject jumpTarget = null;

	public Vector3 GetTargetPosition() {
		return this.jumpTarget.transform.position;
	}

	public void LinkTo(GameObject target) {
		this.jumpTarget = target;
	}

    void OnRenderObject()
    {
       if(FindObjectOfType<DebugArrows>().DrawArrows)
       {
           if (this.jumpTarget)
               DrawArrow.ForDebugTwoPoints(this.transform.position, this.jumpTarget.transform.position, Color.blue, 0.25f, 20f, 1f);
       }
    }
}