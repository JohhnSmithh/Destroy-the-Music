using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHealthBar : MonoBehaviour
{

    // Inspector variables
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject grayBar;

    // Unity variables
    private QuestionManager questionManager;

    // variables
    private float percentage;

    // Start is called before the first frame update
    void Start()
    {
        // Unity Variables
        questionManager = GameObject.Find("QuestionManager").GetComponent<QuestionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        percentage = questionManager.getHealthPercentage();

        healthBar.transform.localPosition = new Vector3(-grayBar.transform.localScale.x * (1-percentage)/2, healthBar.transform.localPosition.y, 1);
        healthBar.transform.localScale = new Vector3(grayBar.transform.localScale.x * percentage, healthBar.transform.localScale.y, 1);
    }
}
