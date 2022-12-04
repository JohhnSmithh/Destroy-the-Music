using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    // Inspector variables
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private GameObject trueButton;
    [SerializeField] private GameObject falseButton;

    // constants
    private const int MAX_HP = 100;
    private const int HP_ON_CORRECT = 10;
    private const int HP_ON_INCORRECT = 5;

    // variables
    private enum QuestionType
    {
        None,
        TrueFalse,
        MultipleChoiceArtist,
        MultipleChoiceAlbum,
        DaneabilitySlider
    }
    private QuestionType questionType;
    private float correctAnswer;
    private int bossHP = 100; // will be replaced by hash map size once implemented

    // Start is called before the first frame update
    void Start()
    {
        questionType = QuestionType.TrueFalse; // temporary

        // initialize all buttons to NOT active
        trueButton.SetActive(false);
        falseButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(questionType == QuestionType.None)
        {
            // put transition here between starting and beginning questions -> with some delay mechanic
        }
        if(questionType == QuestionType.TrueFalse)
        {
            // Set Question Text
            questionText.SetText("True or False? The following song is explicit: ");

            // 1 indicates true, 0 indicates false
            correctAnswer = 1; // replace with reading from hash map object for actual correct answer

            // activate true/false buttons and disable all other UI
            trueButton.SetActive(true);
            falseButton.SetActive(true);
        }

        // add boss HP bar render
    }

    #region BUTTON PRESS FUNCTIONS
    public void truePress()
    {
        if (correctAnswer == 1)
            bossHP-=HP_ON_CORRECT;
        else
        {
            bossHP += HP_ON_INCORRECT;
            if (bossHP > MAX_HP)
                bossHP = MAX_HP; // ensures HP does not exceed max
        }
    }

    public void falsePress()
    {
        if (correctAnswer == 0)
            bossHP -= HP_ON_CORRECT;
        else
        {
            bossHP += HP_ON_INCORRECT;
            if (bossHP > MAX_HP)
                bossHP = MAX_HP; // ensures HP does not exceed max
        }
    }
    #endregion
}
