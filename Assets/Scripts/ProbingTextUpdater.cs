using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProbingTextUpdater : MonoBehaviour
{
    // constants
    private const float redCutoff = 0.007f;
    private const float delay = 0.2f;

    // Unity variables
    private TextMeshProUGUI text;
    private QuestionManager questionManager;

    // Start is called before the first frame update
    void Start()
    {
        // Unity variables
        text = GetComponent<TextMeshProUGUI>();
        questionManager = GameObject.Find("QuestionManager").GetComponent<QuestionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // render probing method stored in game manager and make operation time is above red cutoff threshold
        text.SetText("<u>Probing Method</u>:\n" + (GameManager.instance.getIsLinear() ? "Linear" : "Quadratic")
        + "\n<u>Avg Time/Operation</u>:\n" + (questionManager.getAvgOperationTime() > redCutoff ? 
        "<color=red>" + questionManager.getAvgOperationTime().ToString("0.0000") + "</color>" : questionManager.getAvgOperationTime().ToString("0.0000")));
    }
}
