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
    [SerializeField] private GameObject aButton;
    [SerializeField] private GameObject bButton;
    [SerializeField] private GameObject cButton;
    [SerializeField] private GameObject dButton;

    // constants
    private const int MAX_HP = 100;
    private const int HP_ON_CORRECT = 10;
    private const int HP_ON_INCORRECT = 5;
    private const float NEXT_QUESTION_DELAY = 5f;
    private const float FIRST_SCENE_DELAY = 1f;

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
        nextQuestionTimer = -FIRST_SCENE_DELAY; // creates additional delay on first dialogue

        // set up text/dialogue for before any questions
        questionText.SetText("Welcome music haters to the most popular game show of all time, Destroy The Music! " +
            "Let's see what you've got as we move on to your first challenging question!");

        // initialize all buttons to NOT active
        trueButton.SetActive(false);
        falseButton.SetActive(false);
        aButton.SetActive(false);
        bButton.SetActive(false);
        cButton.SetActive(false);
        dButton.SetActive(false);
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
                // initialize all buttons to NOT active
                trueButton.SetActive(false);
                falseButton.SetActive(false);
                aButton.SetActive(false);
                bButton.SetActive(false);
                cButton.SetActive(false);
                dButton.SetActive(false);

                // randomly select next question type
                questionType = (QuestionType) Random.Range(0, 3);

                if (questionType == QuestionType.TrueFalse)
                {
                    // Set Question Text
                    questionText.SetText("True or False? The following song is explicit: ");

                    // 1 indicates true, 0 indicates false
                    correctAnswer = 1; // replace with reading from random song from hash map object

                    // activate true/false buttons
                    trueButton.SetActive(true);
                    falseButton.SetActive(true);
                    trueButton.GetComponent<Button>().interactable = true;
                    falseButton.GetComponent<Button>().interactable = true;
                    trueButton.transform.GetChild(1).gameObject.SetActive(false);
                    trueButton.transform.GetChild(2).gameObject.SetActive(false);
                    falseButton.transform.GetChild(1).gameObject.SetActive(false);
                    falseButton.transform.GetChild(2).gameObject.SetActive(false);

                    // disable all unrelated UI
                    aButton.SetActive(false);
                    bButton.SetActive(false);
                    cButton.SetActive(false);
                    dButton.SetActive(false);
                }
                else if(questionType == QuestionType.MultipleChoiceAlbum)
                {
                    // Set Question Text
                    questionText.SetText("What is the Album name for the following song: ");

                    // 0 is a, 1 is b, 2 is c, and 3 is d
                    correctAnswer = 0; // load from hash map entry

                    aButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Album A"); // load from hash map entry
                    bButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Album B"); // load from hash map entry
                    cButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Album C"); // load from hash map entry
                    dButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Album D"); // load from hash map entry

                    // activate multiple choice buttons and
                    aButton.SetActive(true);
                    bButton.SetActive(true);
                    cButton.SetActive(true);
                    dButton.SetActive(true);
                    aButton.GetComponent<Button>().interactable = true;
                    bButton.GetComponent<Button>().interactable = true;
                    cButton.GetComponent<Button>().interactable = true;
                    dButton.GetComponent<Button>().interactable = true;
                    aButton.transform.GetChild(1).gameObject.SetActive(true);
                    aButton.transform.GetChild(2).gameObject.SetActive(false);
                    aButton.transform.GetChild(3).gameObject.SetActive(false);
                    bButton.transform.GetChild(1).gameObject.SetActive(true);
                    bButton.transform.GetChild(2).gameObject.SetActive(false);
                    bButton.transform.GetChild(3).gameObject.SetActive(false);
                    cButton.transform.GetChild(1).gameObject.SetActive(true);
                    cButton.transform.GetChild(2).gameObject.SetActive(false);
                    cButton.transform.GetChild(3).gameObject.SetActive(false);
                    dButton.transform.GetChild(1).gameObject.SetActive(true);
                    dButton.transform.GetChild(2).gameObject.SetActive(false);
                    dButton.transform.GetChild(3).gameObject.SetActive(false);

                    // disable all unrelated UI
                    trueButton.SetActive(false);
                    falseButton.SetActive(false);
                }
                else if(questionType == QuestionType.MultipleChoiceArtist)
                {
                    // Set Question Text
                    questionText.SetText("What is the Artist name for the following song: ");

                    // 0 is a, 1 is b, 2 is c, and 3 is d
                    correctAnswer = 0; // load from hash map entry

                    aButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Artist A"); // load from hash map entry
                    bButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Artist B"); // load from hash map entry
                    cButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Artist C"); // load from hash map entry
                    dButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Artist D"); // load from hash map entry

                    // activate multiple choice buttons and
                    aButton.SetActive(true);
                    bButton.SetActive(true);
                    cButton.SetActive(true);
                    dButton.SetActive(true);
                    aButton.GetComponent<Button>().interactable = true;
                    bButton.GetComponent<Button>().interactable = true;
                    cButton.GetComponent<Button>().interactable = true;
                    dButton.GetComponent<Button>().interactable = true;
                    aButton.transform.GetChild(1).gameObject.SetActive(true);
                    aButton.transform.GetChild(2).gameObject.SetActive(false);
                    aButton.transform.GetChild(3).gameObject.SetActive(false);
                    bButton.transform.GetChild(1).gameObject.SetActive(true);
                    bButton.transform.GetChild(2).gameObject.SetActive(false);
                    bButton.transform.GetChild(3).gameObject.SetActive(false);
                    cButton.transform.GetChild(1).gameObject.SetActive(true);
                    cButton.transform.GetChild(2).gameObject.SetActive(false);
                    cButton.transform.GetChild(3).gameObject.SetActive(false);
                    dButton.transform.GetChild(1).gameObject.SetActive(true);
                    dButton.transform.GetChild(2).gameObject.SetActive(false);
                    dButton.transform.GetChild(3).gameObject.SetActive(false);

                    // disable all unrelated UI
                    trueButton.SetActive(false);
                    falseButton.SetActive(false);
                }
                else if(questionType == QuestionType.DaneabilitySlider)
                {
                    // Set Question Text
                    questionText.SetText("Make your best guess at the danceability of the following song: ");

                    correctAnswer = 0.5f; // load from hash map entry

                    // activate confidence slider UI element 

                    // disable all unrelated UI
                    trueButton.SetActive(false);
                    falseButton.SetActive(false);
                    aButton.SetActive(false);
                    bButton.SetActive(false);
                    cButton.SetActive(false);
                    dButton.SetActive(false);
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

            questionText.SetText("Good job! You just destroyed X tracks!");
        }
        else
        {
            bossHP += HP_ON_INCORRECT;
            if (bossHP > MAX_HP)
                bossHP = MAX_HP; // ensures HP does not exceed max

            questionText.SetText("Uh Oh! Y more tracks were just created!");
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

    public void aPress()
    { 
        multipleChoicePress(0);
    }

    public void bPress()
    {
        multipleChoicePress(1);
    }

    public void cPress()
    {
        multipleChoicePress(2);
    }

    public void dPress()
    {
        multipleChoicePress(3);
    }

    private void multipleChoicePress(int letter)
    {
        answered = true;
        nextQuestionTimer = 0;
        aButton.GetComponent<Button>().interactable = false;
        bButton.GetComponent<Button>().interactable = false;
        cButton.GetComponent<Button>().interactable = false;
        dButton.GetComponent<Button>().interactable = false;

        if (correctAnswer == letter)
        {
            bossHP -= HP_ON_CORRECT;
            if (bossHP < 0)
                bossHP = 0;

            questionText.SetText("Good job! You just destroyed X tracks!");
        }
        else
        {
            bossHP += HP_ON_INCORRECT;
            if (bossHP > MAX_HP)
                bossHP = MAX_HP; // ensures HP does not exceed max

            questionText.SetText("Uh Oh! Y more tracks were just created!");
        }

        // show check and x marks to show correct and incorrect answers
        if (correctAnswer == 0)
        {
            aButton.transform.GetChild(3).gameObject.SetActive(true);
            bButton.transform.GetChild(2).gameObject.SetActive(true);
            cButton.transform.GetChild(2).gameObject.SetActive(true);
            dButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (correctAnswer == 1)
        {
            aButton.transform.GetChild(2).gameObject.SetActive(true);
            bButton.transform.GetChild(3).gameObject.SetActive(true);
            cButton.transform.GetChild(2).gameObject.SetActive(true);
            dButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (correctAnswer == 2)
        {
            aButton.transform.GetChild(2).gameObject.SetActive(true);
            bButton.transform.GetChild(2).gameObject.SetActive(true);
            cButton.transform.GetChild(3).gameObject.SetActive(true);
            dButton.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (correctAnswer == 3)
        {
            aButton.transform.GetChild(2).gameObject.SetActive(true);
            bButton.transform.GetChild(2).gameObject.SetActive(true);
            cButton.transform.GetChild(2).gameObject.SetActive(true);
            dButton.transform.GetChild(3).gameObject.SetActive(true);
        }
        // stop showing letter labels
        aButton.transform.GetChild(1).gameObject.SetActive(false);
        bButton.transform.GetChild(1).gameObject.SetActive(false);
        cButton.transform.GetChild(1).gameObject.SetActive(false);
        dButton.transform.GetChild(1).gameObject.SetActive(false);
    }
    #endregion
}
