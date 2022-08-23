using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputField : MonoBehaviour
{
    // static identifier for input field
    public static InputField instance;
    // last number field clicked
    NumberField lastField;

    /// <summary>
    /// Make sure to only have one instance of this input field
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // hide input field
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Activate input field if a number field is clicked
    /// </summary>
    /// <param name="field">The clicked number field</param>
    public void ActivateInputField(NumberField field)
    {
        // show input field
        this.gameObject.SetActive(true);
        // set the last field as this one
        lastField = field;
    }

    /// <summary>
    /// Set the value into the grid,
    /// when an input field is clicked
    /// </summary>
    /// <param name="number">Input value</param>
    public void ClickedInput(int number)
    {
        // grid should receive input
        lastField.GetInput(number);
        // hide input panel
        this.gameObject.SetActive(false);
        // hide hint panel
        Hint.instance.gameObject.SetActive(false);
        VoiceRecognition.instance.lastField = null;
    }

    /// <summary>
    /// Hide the input field
    /// </summary>
    public void HideInput()
    {
        this.gameObject.SetActive(false);
    }
}
