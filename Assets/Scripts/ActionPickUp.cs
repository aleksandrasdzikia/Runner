using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPickUp : MonoBehaviour, IAction
{
	public void executePickUp(Player player) {
		foreach (IPickUp pickUp in this.GetComponents<IPickUp>())
			pickUp.execute(player);
	}
}