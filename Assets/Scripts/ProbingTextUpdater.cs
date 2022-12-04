using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProbingTextUpdater : MonoBehaviour
{
    // Unity variables
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        // Unity variables
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText("Probing Method:\n" + (GameManager.instance.getIsLinear()?"Linear":"Quadratic"));
    }
}
