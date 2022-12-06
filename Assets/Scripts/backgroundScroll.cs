using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Credit for this simple scripting solution to background scrolling: https://www.youtube.com/watch?v=-6H-uYh80vc
public class backgroundScroll : MonoBehaviour
{
    // inspector variables
    [SerializeField] private RawImage img;
    [SerializeField] private float x, y;

    // Update is called once per frame
    void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(x, y)*Time.deltaTime, img.uvRect.size);
    }
}
