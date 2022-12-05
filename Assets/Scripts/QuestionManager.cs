using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private const float NEXT_QUESTION_DELAY = 5f;

    // variables
    private enum QuestionType
    {
        TrueFalse,
        MultipleChoiceArtist,
        MultipleChoiceAlbum,
        DaneabilitySlider
    }
    private QuestionType questionType;
    private float correctAnswer;
    private int bossHP = 100; // will be replaced by hash map size once implemented
    private bool answered;
    private float nextQuestionTimer;

    // Start is called before the first frame update
    void Start()
    {
        // variables
        answered = true;
        nextQuestionTimer = -5; // creates additional 5 second delay on first dialogue

        // initialize all buttons to NOT active
        trueButton.SetActive(false);
        falseButton.SetActive(false);

        // set up text/dialogue for before any questions
        questionText.SetText("Welcome music haters to the most popular game show of all time, Destroy The Music! " +
            "Let's see what you've got as we move on to your first challenging question!");
    }

    // Update is called once per frame 
    void Update()
    {
        if(answered)
        {
            nextQuestionTimer += Time.deltaTime;

            // load next random questiona after timer delay
            if(nextQuestionTimer > NEXT_QUESTION_DELAY)
            {
                // randomly select next question type
                questionType = (QuestionType) Random.Range(0, 1);

                if (questionType == QuestionType.TrueFalse)
                {
                    // Set Question Text
                    questionText.SetText("True or False? The following song is explicit: ");

                    // 1 indicates true, 0 indicates false
                    correctAnswer = 1; // replace with reading from random song from hash map object

                    // activate true/false buttons and disable all other UI
                    trueButton.SetActive(true);
                    falseButton.SetActive(true);
                    trueButton.GetComponent<Button>().interactable = true;
                    falseButton.GetComponent<Button>().interactable = true;
                    trueButton.transform.GetChild(1).gameObject.SetActive(false);
                    trueButton.transform.GetChild(2).gameObject.SetActive(false);
                    falseButton.transform.GetChild(1).gameObject.SetActive(false);
                    falseButton.transform.GetChild(2).gameObject.SetActive(false);

                }
                else if(questionType == QuestionType.MultipleChoiceAlbum)
                {
                    // Set Question Text
                    questionText.SetText("What is the Album name for the following song: ");

                    // 0 is a, 1 is b, 2 is c, and 3 is d
                    correctAnswer = 0;

                    // activate multiple choice buttons and disable all other UI
                    trueButton.SetActive(false);
                    falseButton.SetActive(false);
                }
                else if(questionType == QuestionType.MultipleChoiceArtist)
                {
                    // Set Question Text
                    questionText.SetText("What is the Artist name for the following song: ");

                    // 0 is a, 1 is b, 2 is c, and 3 is d
                    correctAnswer = 0;

                    // activate multiple choice buttons and disable all other UI
                    trueButton.SetActive(false);
                    falseButton.SetActive(false);
                }
                else if(questionType == QuestionType.DaneabilitySlider)
                {
                    // Set Question Text
                    questionText.SetText("Make your best guess at the danceability of the following song: ");

                    correctAnswer = 0.5f;

                    // activate confidence slider and disable all other UI
                    trueButton.SetActive(false);
                    falseButton.SetActive(false);
                }

                answered = false;
            }
        }
    }

    public float getHealthPercentage()
    {
        return (float) bossHP / MAX_HP;
    }

    #region BUTTON PRESS FUNCTIONS
    public void truePress()
    {
        trueFalsePress(true);
    }

    public void falsePress()
    {
        trueFalsePress(false);
    }

    private void trueFalsePress(bool buttonType)
    {
        answered = true;
        nextQuestionTimer = 0;
        trueButton.GetComponent<Button>().interactable = false;
        falseButton.GetComponent<Button>().interactable = false;

        if (correctAnswer == (buttonType ? 1 : 0))
        {
            bossHP -= HP_ON_CORRECT;
            if (bossHP < 0)
                bossHP = 0;
        }
        else
        {
            bossHP += HP_ON_INCORRECT;
            if (bossHP > MAX_HP)
                bossHP = MAX_HP; // ensures HP does not exceed max
        }

        // show check and x marks to show correct and incorrect answers
        if (correctAnswer == 1)
        {
            trueButton.transform.GetChild(2).gameObject.SetActive(true);
            falseButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            trueButton.transform.GetChild(1).gameObject.SetActive(true);
            falseButton.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    #endregion
}
