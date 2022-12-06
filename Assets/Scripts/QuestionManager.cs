using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private TextMeshProUGUI rangeText;

    // Unity variables
    private Animator announcerAnimator;

    // constants
    private const int MAX_HP = 125000;
    private const int HP_ON_CORRECT = 20000;
    private const int HP_ON_INCORRECT = 5000;
    private const float NEXT_QUESTION_DELAY = 3f;
    private const float FIRST_SCENE_DELAY = 6f;

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
    private bool answered;
    private float nextQuestionTimer;
    private float rangeCenterGuess;
    private bool gameDone;
    private float gameDoneTimer;
    private bool lastQuestionCorrect;

    //HashMap
    HashMap map = new();

    // Start is called before the first frame update
    void Start()
    {
        // variables
        answered = true;
        nextQuestionTimer = -FIRST_SCENE_DELAY; // creates additional delay on first dialogue
        gameDone = false;
        gameDoneTimer = 0;
        lastQuestionCorrect = false;

        // unity variables
        announcerAnimator = GameObject.Find("Announcer").GetComponent<Animator>();

        // set up text/dialogue for before any questions
        int tempRand = Random.Range(0, 2);
        if (tempRand == 0)
            questionText.SetText("Don’t cue it up, or I’ll get sick, because tonight we’re going to DESTROY THE MUSIC! Welcome back, music haters - " +
                "let’s give our new contestant some questions right away so we can turn that background music off!");
        else
            questionText.SetText("It’s time to stop the showtunes, it’s time to kill the lights - it’s time to DESTROY THE MUSIC on our game show tonight! Welcome back, music haters - " +
                "let’s give our new contestant some questions right away so we can turn that background music off!");

        // initialize all buttons to NOT active
        trueButton.SetActive(false);
        falseButton.SetActive(false);
        aButton.SetActive(false);
        bButton.SetActive(false);
        cButton.SetActive(false);
        dButton.SetActive(false);
        slider.gameObject.SetActive(false);
        confirmButton.SetActive(false);
        rangeText.gameObject.SetActive(false);

        // announcer talks at start
        GameManager.instance.PlayRandomVoiceAudio();
        announcerAnimator.SetTrigger("talk");

        //Initialize the HashMap
        map.Initialize(GameManager.instance.getIsLinear());
    }

    // Update is called once per frame 
    void Update()
    {
        if(gameDone) // count down for leaving scene
        {
            if (gameDoneTimer >= FIRST_SCENE_DELAY)
                SceneManager.LoadScene("MenuScene");

            gameDoneTimer += Time.deltaTime;
        }
        else
        {
            // victory condition
            if (map.GetSize() == 0)
            {
                // set all buttons to NOT active
                trueButton.SetActive(false);
                falseButton.SetActive(false);
                aButton.SetActive(false);
                bButton.SetActive(false);
                cButton.SetActive(false);
                dButton.SetActive(false);
                slider.gameObject.SetActive(false);
                confirmButton.SetActive(false);
                rangeText.gameObject.SetActive(false);

                // reset to proper text box texture
                questionText.transform.GetChild(1).gameObject.SetActive(false);
                questionText.transform.GetChild(2).gameObject.SetActive(true);
                questionText.transform.GetChild(3).gameObject.SetActive(false);
                questionText.transform.GetChild(4).gameObject.SetActive(false);

                // show victory text
                questionText.SetText("YES! You did it! Every single music track, GONE! HAHAHAHAHA!!!!");

                gameDone = true;
            }
            // failure condition
            else if (map.GetSize() >= MAX_HP)
            {
                // set all buttons to NOT active
                trueButton.SetActive(false);
                falseButton.SetActive(false);
                aButton.SetActive(false);
                bButton.SetActive(false);
                cButton.SetActive(false);
                dButton.SetActive(false);
                slider.gameObject.SetActive(false);
                confirmButton.SetActive(false);
                rangeText.gameObject.SetActive(false);

                // reset to proper text box texture
                questionText.transform.GetChild(1).gameObject.SetActive(false);
                questionText.transform.GetChild(2).gameObject.SetActive(true);
                questionText.transform.GetChild(3).gameObject.SetActive(false);
                questionText.transform.GetChild(4).gameObject.SetActive(false);

                // show failure text
                questionText.SetText("No, no, no!! Now there's even MORE music! Didn't you read the title of the show?!");

                gameDone = true;
            }
            else // check for answered state
            {
                if (answered)
                {
                    nextQuestionTimer += Time.deltaTime;

                    // load next random questiona after timer delay
                    if (nextQuestionTimer > NEXT_QUESTION_DELAY)
                    {
                        // announcer says next question
                        GameManager.instance.PlayRandomVoiceAudio();
                        announcerAnimator.SetTrigger("talk");

                        // initialize all buttons to NOT active
                        trueButton.SetActive(false);
                        falseButton.SetActive(false);
                        aButton.SetActive(false);
                        bButton.SetActive(false);
                        cButton.SetActive(false);
                        dButton.SetActive(false);
                        slider.gameObject.SetActive(false);
                        confirmButton.SetActive(false);
                        rangeText.gameObject.SetActive(false);

                        // reset to proper text box texture
                        questionText.transform.GetChild(1).gameObject.SetActive(false);
                        questionText.transform.GetChild(2).gameObject.SetActive(true);
                        questionText.transform.GetChild(3).gameObject.SetActive(false);
                        questionText.transform.GetChild(4).gameObject.SetActive(false);

                        // randomly select next question type
                        questionType = (QuestionType)Random.Range(0, 4);

                        if (questionType == QuestionType.TrueFalse)
                        {
                            Song song = map.GetRandomSong();

                            // Set Question Text
                            questionText.SetText("The following song is <b>explicit</b>. True or False?: <b>" + song.getName() + "</b>");

                            // 1 indicates true, 0 indicates false
                            correctAnswer = song.isExplicit() ? 1 : 0; // replace with reading from random song from hash map object

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
                            slider.gameObject.SetActive(false);
                            confirmButton.SetActive(false);
                            rangeText.gameObject.SetActive(false);
                        }
                        else if (questionType == QuestionType.MultipleChoiceAlbum)
                        {
                            // get 4 songs
                            List<Song> songs = map.GetRandomSongs();

                            // 0 is a, 1 is b, 2 is c, and 3 is d
                            correctAnswer = Random.Range(0, 4);

                            // Set Question Text
                            questionText.SetText("What is the <b>Album</b> name for the following song: <b>" + songs[(int)correctAnswer].getName() + "</b>");

                            aButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[0].getAlbum()); // load from hash map entry
                            bButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[1].getAlbum()); // load from hash map entry
                            cButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[2].getAlbum()); // load from hash map entry
                            dButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[3].getAlbum()); // load from hash map entry

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
                            slider.gameObject.SetActive(false);
                            confirmButton.SetActive(false);
                            rangeText.gameObject.SetActive(false);
                        }
                        else if (questionType == QuestionType.MultipleChoiceArtist)
                        {
                            // get 4 songs
                            List<Song> songs = map.GetRandomSongs();

                            // 0 is a, 1 is b, 2 is c, and 3 is d
                            correctAnswer = Random.Range(0, 4);

                            // Set Question Text
                            questionText.SetText("What is the <b>Artist</b> name for the following song: <b>" + songs[(int)correctAnswer].getName() + "</b>");

                            aButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[0].getArtist()); // load from hash map entry
                            bButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[1].getArtist()); // load from hash map entry
                            cButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[2].getArtist()); // load from hash map entry
                            dButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(songs[3].getArtist()); // load from hash map entry

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
                            slider.gameObject.SetActive(false);
                            confirmButton.SetActive(false);
                            rangeText.gameObject.SetActive(false);
                        }
                        else if (questionType == QuestionType.DaneabilitySlider)
                        {
                            // get one song
                            Song song = map.GetRandomSong();

                            // Set Question Text
                            questionText.SetText("Make your best guess at the <b>danceability</b> of the following song: <b>" + song.getName() + "</b>");

                            correctAnswer = (float)song.getDanceability(); // load correct answer from hash map entry

                            // activate confidence slider UI element 
                            slider.gameObject.SetActive(true);
                            confirmButton.SetActive(true);
                            rangeText.gameObject.SetActive(true);
                            slider.interactable = true;
                            confirmButton.GetComponent<Button>().interactable = true;
                            confirmButton.transform.GetChild(1).gameObject.SetActive(false);
                            confirmButton.transform.GetChild(2).gameObject.SetActive(false);
                            slider.value = 0.5f;

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
                else if (questionType == QuestionType.DaneabilitySlider) // if question is still going, update text slider to appropriate current range
                {
                    rangeCenterGuess = mapRange(slider.value, 0f, 1f, 0.1f, 0.9f); // map from slider scale to guess range scale
                    rangeText.SetText("Current Range: " + (rangeCenterGuess - 0.1f).ToString("0.00") + "-" + (rangeCenterGuess + 0.1f).ToString("0.00"));
                }
            }
        }

        // set menu announcer to idle animation if not talking
        if (!GameManager.instance.IsTalking())
            announcerAnimator.SetTrigger("idle");

        // set volume level
        GameManager.instance.setMusicVolume(getHealthPercentage());
    }

    public float getHealthPercentage()
    {
        return (float) map.GetSize() / MAX_HP;
    }

    // maps one range to another; see https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    private float mapRange(float val, float from1, float to1, float from2, float to2)
    {
        return (val - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public float getAvgOperationTime()
    {
        return lastQuestionCorrect ? (float) map.GetLastRemoveRuntime() : (float) map.GetLastAddRuntime();
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
        // announcer says results
        GameManager.instance.PlayRandomVoiceAudio();

        // disable standard bubble
        questionText.transform.GetChild(2).gameObject.SetActive(false);

        answered = true;
        nextQuestionTimer = 0;
        trueButton.GetComponent<Button>().interactable = false;
        falseButton.GetComponent<Button>().interactable = false;

        if (correctAnswer == (buttonType ? 1 : 0))
        {
            map.RemoveX(HP_ON_CORRECT);

            // set question text in response to answer
            showQuestionRightText(HP_ON_CORRECT);

            // enable correct bubble
            questionText.transform.GetChild(3).gameObject.SetActive(true);

            // announcer talk
            announcerAnimator.SetTrigger("talk");

            lastQuestionCorrect = true;
        }
        else
        {
            map.AddX(HP_ON_INCORRECT);

            // set question text in response to answer
            showQuestionWrongText(HP_ON_INCORRECT);

            // enable incorrect bubble
            questionText.transform.GetChild(4).gameObject.SetActive(true);

            // announcer shock
            announcerAnimator.SetTrigger("shock");

            lastQuestionCorrect = false;
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
        // announcer says results
        GameManager.instance.PlayRandomVoiceAudio();

        // disable standard bubble
        questionText.transform.GetChild(2).gameObject.SetActive(false);

        answered = true;
        nextQuestionTimer = 0;
        aButton.GetComponent<Button>().interactable = false;
        bButton.GetComponent<Button>().interactable = false;
        cButton.GetComponent<Button>().interactable = false;
        dButton.GetComponent<Button>().interactable = false;

        if (correctAnswer == letter)
        {
            map.RemoveX(HP_ON_CORRECT);

            // set question text in response to answer
            showQuestionRightText(HP_ON_CORRECT);

            // enable correct bubble
            questionText.transform.GetChild(3).gameObject.SetActive(true);

            // announcer talk
            announcerAnimator.SetTrigger("talk");

            lastQuestionCorrect = true;
        }
        else
        {
            map.AddX(HP_ON_INCORRECT);

            // set question text in response to answer
            showQuestionWrongText(HP_ON_INCORRECT);

            // enable incorrect bubble
            questionText.transform.GetChild(4).gameObject.SetActive(true);

            // announcer shock
            announcerAnimator.SetTrigger("shock");

            lastQuestionCorrect = false;
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

    public void confirmPress()
    {
        // announcer says results
        GameManager.instance.PlayRandomVoiceAudio();

        // disable standard bubble
        questionText.transform.GetChild(2).gameObject.SetActive(false);

        answered = true;
        nextQuestionTimer = 0;
        confirmButton.GetComponent<Button>().interactable = false;
        slider.interactable = false;

        if (correctAnswer >= rangeCenterGuess - 0.1f && correctAnswer <= rangeCenterGuess + 0.1f)
        {
            map.RemoveX(HP_ON_CORRECT);

            // set question text in response to answer
            showQuestionRightText(HP_ON_CORRECT);

            // show check mark and show correct answer
            confirmButton.transform.GetChild(2).gameObject.SetActive(true);
            rangeText.SetText("Current Range: " + (rangeCenterGuess - 0.1f).ToString("0.00") + "-" + (rangeCenterGuess + 0.1f).ToString("0.00")
            + "\n<color=green>Correct Answer: " + correctAnswer.ToString("0.00") +"</color>");

            // enable correct bubble
            questionText.transform.GetChild(3).gameObject.SetActive(true);

            // announcer talk
            announcerAnimator.SetTrigger("talk");

            lastQuestionCorrect = true;
        }
        else
        {
            map.AddX(HP_ON_INCORRECT);

            // set question text in response to answer
            showQuestionWrongText(HP_ON_INCORRECT);

            // show check mark and show incorrect answer
            confirmButton.transform.GetChild(1).gameObject.SetActive(true);
            rangeText.SetText("Current Range: " + (rangeCenterGuess - 0.1f).ToString("0.00") + "-" + (rangeCenterGuess + 0.1f).ToString("0.00")
            + "\n<color=red>Correct Answer: " + correctAnswer.ToString("0.00") + "</color>");

            // enable incorrect bubble
            questionText.transform.GetChild(4).gameObject.SetActive(true);

            // announcer shock
            announcerAnimator.SetTrigger("shock");

            lastQuestionCorrect = false;
        }
    }

    private void showQuestionRightText(int removedSongs)
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
            questionText.SetText("Great job! That's " + removedSongs + " more songs gone!");
        else
            questionText.SetText("Good job! You just obliterated " + removedSongs + " tracks!");
    }

    private void showQuestionWrongText(int addedSongs)
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
            questionText.SetText("Oh no! You just added " + addedSongs + " more tracks!");
        else
            questionText.SetText(addedSongs + " new songs were just added!?! That's too many!");
    }

    #endregion
}
