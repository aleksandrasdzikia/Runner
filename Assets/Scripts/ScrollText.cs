using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollText : MonoBehaviour {

    public float Scroll_y = 0.5f;

    // Update is called once per frame
    void Update () {

        float OffsetY = Time.time * Scroll_y;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, OffsetY);
    }
}
