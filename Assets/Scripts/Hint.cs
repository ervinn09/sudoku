using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    // static identifier for hint button
    public static Hint instance;
    // make connection to these and change data as it runs
    public GameObject msgWindows;
    public TextMeshProUGUI noHints;

    NumberField lastField; // last number field clicked
    int maxHints = 3; // max number of hints left   

    /// <summary>
    /// Make sure to only have one instance of this hint button
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // hide hint button and no hint left text
        this.gameObject.SetActive(false);
        noHints.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activate hint button if a number field is clicked,
    /// only if there is hint left
    /// </summary>
    /// <param name="field">The clicked number field</param>
    public void ActivateHintBtn(NumberField field)
    {
        if (maxHints > 0)
        {
            // show hint button
            this.gameObject.SetActive(true);
            // set the last field as this one
            lastField = field;
        }
        else
        {
            // hide hint button and show no hint left
            this.gameObject.SetActive(false);
            noHints.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Set the hinted value into the grid,
    /// when hint button is clicked
    /// </summary>
    public void SetValue()
    {
        if (maxHints > 0)
        {
            maxHints--;
            SudokuGrid.instance.ShowHint(ref lastField);
            InputField.instance.HideInput();
            this.gameObject.SetActive(false);
        }
    }
}
