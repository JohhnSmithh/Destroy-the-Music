using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHealthBar : MonoBehaviour
{

    // constants
    private const float WIDTH = 17.46591f;
    private const float FACTOR = 3.192669435f;

    // Inspector variables
    [SerializeField] private GameObject healthBar;

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

        healthBar.transform.localPosition = new Vector3(-WIDTH * (1-percentage)/2, healthBar.transform.localPosition.y, 1);
        healthBar.transform.localScale = new Vector3(WIDTH * percentage / FACTOR, healthBar.transform.localScale.y, 1);
    }
}
