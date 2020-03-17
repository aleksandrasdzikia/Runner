using UnityEngine;
using System.Collections;

public static class Res {
    public static Hashtable res = new Hashtable();
    public static Hashtable levels = new Hashtable();
    //public static Hashtable maps = new Hashtable();
    //public static Hashtable gold = new Hashtable();
    //public static Hashtable vines = new Hashtable();

    public static void InitResources() {

        res.Add("points", Resources.Load<GameObject>("_Points"));
        //maps.Add("points", Resources.Load<GameObject>("Points"));

        levels.Add("c0", Resources.Load<GameObject>("Jungle Level/0"));
        levels.Add("c1", Resources.Load<GameObject>("Jungle Level/1"));
        levels.Add("c2", Resources.Load<GameObject>("Jungle Level/2"));
        levels.Add("c3", Resources.Load<GameObject>("Jungle Level/3"));
        levels.Add("c4", Resources.Load<GameObject>("Jungle Level/4"));


        //--------------------------------------------------------------------------------------------------------------
        //levels.Add("c1", Resources.Load<GameObject>("old/oldLevels/1"));
        //levels.Add("c2", Resources.Load<GameObject>("old/oldLevels/2"));
        //levels.Add("c3", Resources.Load<GameObject>("old/oldLevels/3"));

        /* //----------------COMENTED WHOLE STATEMENT-------------------------------------------------------------------

        levels.Add("l3", Resources.Load<GameObject>("1"));
        levels.Add("r3", Resources.Load<GameObject>("2"));

        levels.Add("l4", Resources.Load<GameObject>("3"));
        levels.Add("r4", Resources.Load<GameObject>("4"));

        levels.Add("l5", Resources.Load<GameObject>("5"));
        levels.Add("r5", Resources.Load<GameObject>("6"));

        

        for (int i = 0; i < 4; i++) {
            gold.Add("l3g" + i, Resources.Load<GameObject>("Road stuff/Bag Coins/Left_03_Coins " + i));
            gold.Add("l4g" + i, Resources.Load<GameObject>("Road stuff/Bag Coins/Left_04_Coins " + i));
            gold.Add("r3g" + i, Resources.Load<GameObject>("Road stuff/Bag Coins/Right_03_Coins " + i));
            gold.Add("r4g" + i, Resources.Load<GameObject>("Road stuff/Bag Coins/Right_04_Coins " + i));
        }

        for (int i = 0; i < 4; i++) {
            vines.Add("l3v" + i, Resources.Load<GameObject>("Road stuff/Vine Obstacles/Left_03_Obs " + i));
            vines.Add("l4v" + i, Resources.Load<GameObject>("Road stuff/Vine Obstacles/Left_04_Obs " + i));
            vines.Add("r3v" + i, Resources.Load<GameObject>("Road stuff/Vine Obstacles/Right_03_Obs " + i));
            vines.Add("r4v" + i, Resources.Load<GameObject>("Road stuff/Vine Obstacles/Right_04_Obs " + i));
        }

    */ //----------------COMENTED WHOLE STATEMENT----------------------------------------------------------------------------

    }

    public static GameObject GetRes(string name) {
        return (GameObject)Res.res[name];
    }

    public static GameObject GetLevel(string name) {
        return (GameObject)Res.levels[name];
    }

    /* //----------------COMENTED WHOLE STATEMENT-------------------------------------------------
    
    public static GameObject GetGold(string name) {
        return (GameObject)Res.gold[name];
    }

    public static GameObject GetVine(string name) {
        return (GameObject)Res.vines[name];
    }

    */ //----------------COMENTED WHOLE STATEMENT--------------------------------------------------
}