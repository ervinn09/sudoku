using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberField : MonoBehaviour
{
    SudokuGrid sudokuGrid;
    // Coordinates of the number field
    int x1, y1;
    // value of the number field
    int value;
    // identifier of the number field
    public string id;
    // connection to the text field of the number field
    public TextMeshProUGUI number;

    /// <summary>
    /// Set the value in the field
    /// </summary>
    /// <param name="x">x coordinate of the row</param>
    /// <param name="y">y coordinate of the column</param>
    /// <param name="value">value to be set</param>
    /// <param name="identifier">identifier of the field</param>
    /// <param name="grid">reference to the grid</param>
    public void SetValues(int x, int y, int value, string identifier, SudokuGrid grid)
    {
        // set everything to the member variables
        this.x1 = x;
        this.y1 = y;
        this.value = value;
        this.id = identifier;
        this.sudokuGrid = grid;

        // set the text in the number field
        number.text = (value != 0) ? value.ToString() : "";

        // make the number field interractable if it's empty
        if (value != 0)
            GetComponentInParent<Button>().interactable = false;
        else
            number.color = Color.blue;
    }

    /// <summary>
    /// Activate the input field and hint button,
    /// whenever the number field is clicked
    /// </summary>
    public void ButtonClick()
    {
        VoiceRecognition.instance.lastField = this;
        InputField.instance.ActivateInputField(this);
        Hint.instance.ActivateHintBtn(this);
    }

    /// <summary>
    /// Get the input from the input field
    /// </summary>
    /// <param name="newValue"></param>
    public void GetInput(int newValue)
    {
        value = newValue;
        number.text = (value != 0) ? value.ToString() : "";

        // Update riddle grid
        sudokuGrid.SetInputInRiddleGrid(x1, y1, value);
    }

    /// <summary>
    /// Get X Coordinate of the number field
    /// </summary>
    /// <returns>The x coordinate of the number field</returns>
    public int GetX()
    {
        return x1;
    }

    /// <summary>
    /// Get Y Coordinate of the number field
    /// </summary>
    /// <returns>The y coordinate of the number field</returns>
    public int GetY()
    {
        return y1;
    }

    /// <summary>
    /// Set the number field with value from hint 
    /// as green and not interactable
    /// </summary>
    /// <param name="value">value from hint</param>
    public void SetHint(int value)
    {
        this.value = value;
        number.text = value.ToString();
        number.color = Color.green;
        GetComponentInParent<Button>().interactable = false;
    }
}
