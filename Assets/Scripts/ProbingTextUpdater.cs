using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProbingTextUpdater : MonoBehaviour
{
    // constants
    private const int redCutoff = 500;
    private const float delay = 0.2f;

    // variables
    private float delayTimer;

    // Unity variables
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        // variables
        delayTimer = delay+0.01f; // ensures automatic text setting in first frame

        // Unity variables
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(delayTimer > delay)
        {
            // render probing method stored in game manager and make FPS red if below cutoff threshold
            text.SetText("<u>Probing Method</u>:\n" + (GameManager.instance.getIsLinear() ? "Linear" : "Quadratic")
            + "\n<u>FPS</u>: " + ((int)(1.0f / Time.deltaTime) < redCutoff ? "<color=red>" + ((int)(1.0f / Time.deltaTime)).ToString() + "</color>" : (int)(1.0f / Time.deltaTime)));

            delayTimer = 0;
        }
        

        delayTimer += Time.deltaTime;
    }
}
