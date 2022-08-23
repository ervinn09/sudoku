using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;

    // dictionary of the known keywords
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private string latestSpeechEvent;

    // Holds current cell after a speech event of type (a .. 1)
    private string selectedRow;
    private string selectedColumn;

    // Holds a number after a speech event of type (1)
    private int selectedNumber;

    // last number field chosen
    public NumberField lastField;

    // static identifier for input field
    public static VoiceRecognition instance;

    /// <summary>
    /// Make sure to only have one instance of this voice recognizer
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Defines speech use cases that will be handled.
    /// </summary>
    void Start()
    {
        // start or play
        actions.Add("play", handleSpeechEvent);
        actions.Add("start", handleSpeechEvent);

        // id of number field a1-i9
        for (int c = 'a'; c <= 'i'; c++)
        {
            for(int d = 1; d <= 9;  d++)
            {
                string speechEvent = (char)c + " " + d.ToString();
                actions.Add(speechEvent, handleSpeechEvent);
            }
        }

        // the number for the number field 1-9
        for (int d = 1; d <= 9; d++)
        {
            string speechEvent = d.ToString();
            actions.Add(speechEvent, handleSpeechEvent);
        }

        // clear number field
        actions.Add("clear", handleSpeechEvent);

        // get hint for the number field
        actions.Add("hint", handleSpeechEvent);

        // submit
        actions.Add("submit", handleSpeechEvent);

        // replay
        actions.Add("play again", handleSpeechEvent);
        actions.Add("restart", handleSpeechEvent);
        actions.Add("replay", handleSpeechEvent);

        // exit game
        actions.Add("exit", handleSpeechEvent);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray(), ConfidenceLevel.Low);
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    /// <summary>
    /// Defines how the use cases will be handled.
    /// </summary>
    private void handleSpeechEvent()
    {
        switch (latestSpeechEvent)
        {
            case "play":
            case "start":
                // only possible in start scene, same as clicked play btn
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("StartScene"))
                    this.gameObject.GetComponentInChildren<Button>().onClick.Invoke();
                break;
            case "clear":
                // only possible in game scene and appropriate number field was chosen
                if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene") &&
                    lastField != null)
                {
                    // clear number field
                    if (lastField.GetComponentInParent<Button>().interactable)
                    {
                        InputField.instance.ClickedInput(0);
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }
                break;
            case "hint":
                // only possible in game scene and appropriate number field was chosen
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene") &&
                    lastField != null)
                {
                    // get hint for number field
                    Hint.instance.SetValue();
                }
                break;
            case "submit":
                // only possible in game scene
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
                {
                    // same as clicked submit btn
                    this.gameObject.GetComponentsInChildren<Button>().First(x => x.name == "CheckButton").Select();
                    this.gameObject.GetComponentsInChildren<Button>().First(x => x.name == "CheckButton").onClick.Invoke();
                }
                break;
            case "play again":
            case "restart":
            case "replay":
                // only possible in game scene and when DoneWindow is visible
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene") &&
                    SudokuGrid.instance.msgWindows.activeInHierarchy)
                {
                    // same as clicked play again btn
                    SudokuGrid.instance.msgWindows.GetComponentsInChildren<Button>().First(x => x.name == "Restart").onClick.Invoke();
                }
                break;
            case "exit":
                // quit app
                Application.Quit();
                break;
            default:
                string[] speech = latestSpeechEvent.Split(' ');

                if (speech.Length > 1)
                {
                    selectedRow = speech[0];
                    selectedColumn = speech[1];
                    selectedNumber = -1;

                    mimicButtonClick(selectedRow + selectedColumn);
                    if (selectedRow != null) Debug.Log("Row:" + selectedRow);
                    if (selectedColumn != null) Debug.Log("Column:" + selectedColumn);
                }
                else
                {
                    selectedNumber = Int32.Parse(speech[0]);
                    // Mimic inputfield if voice rec.
                    if (selectedNumber > 0 && lastField != null)
                    {
                        if (lastField.GetComponentInParent<Button>().interactable)
                        {
                            InputField.instance.ClickedInput(selectedNumber);
                            EventSystem.current.SetSelectedGameObject(null);
                            lastField = null;
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// Invokes function mapped to speech event.
    /// </summary>
    /// <param name="speech"> The speech event. </param>
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        latestSpeechEvent = speech.text;
        actions[speech.text].Invoke();
    }

    /// <summary>
    /// Stops voice recognition.
    /// </summary>
    private void stopVoiceRecognition()
    {
        keywordRecognizer.Stop();
    }

    /// <summary>
    /// Stops voice recognition.
    /// </summary>
    /// <param name="id"> Id of the input buttons on the sudoku grid. </param>
    private void mimicButtonClick(string id)
    {

        for (char c = 'a'; c <= 'i'; c++)
        {
            for (int j = 1; j <= 9; j++)
            {
                // lower letters
                string buttonId = c.ToString() + j;
                if (id.Equals(buttonId))
                {
                    // captial letters (necessary to match id)
                    buttonId = c.ToString().ToUpper() + j;
                    Button numField_btn = SudokuGrid.instance.numberFields.First(field => field.id.Equals(buttonId)).GetComponentInParent<Button>();
                    numField_btn.Select();

                    if (numField_btn.interactable)
                    {
                        // if the btn is interactable,
                        // then it's possible to fill it w/ numbers
                        numField_btn.onClick.Invoke();
                    }
                    else
                    {
                        // btn not interactable,
                        // don't do anything here
                        lastField = null;
                    }
                }
            }
        }
    }
}
