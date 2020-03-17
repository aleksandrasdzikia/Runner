using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionOld {

    private List<Vector3> actionList = new List<Vector3>();

    private Vector3 random;
    private Vector3 goTo;

    public string actionType;

    public ActionOld(Vector3 goTo, List<Vector3> directions, string actionType) {
        actionList = directions;

        this.goTo = goTo;
        this.actionType = actionType;

        if (actionList.Count != 0)
            this.random = actionList[Random.Range(0, actionList.Count)] ;
        else this.random = Vector3.zero;
    }

    public Vector3 getTarget(Vector3 from) {
        return from + this.random;
    }

    public Vector3 direction {
        get { return this.random; }
    }

    public Vector3 getGoTo() {
        return this.goTo;
    }

    public string type {
        get { return this.actionType; }
    }
}
