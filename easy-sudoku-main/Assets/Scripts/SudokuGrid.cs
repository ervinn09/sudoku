using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SudokuGrid : MonoBehaviour
{
    int[,] solvedGrid = new int[9, 9]; // 9x9 fields full
    int[,] riddleGrid = new int[9, 9]; // 9x9 fields to play
    string debugLog; // string for debug purposes

    // static identifier for this sudoku grid
    public static SudokuGrid instance;
    // parent the created buttons directly to the transforms of our 3x3 blocks
    public Transform A1, A2, A3, B1, B2, B3, C1, C2, C3;
    // prefab of button to have connection to these and fill the data on creation
    public GameObject buttonPrefab;
    // make connection to these and change data as it runs
    public GameObject msgWindows;
    public GameObject gridLabel;
    public GameObject inputField;
    public Button submitBtn;
    public Button hintBtn;
    public TextMeshProUGUI noHints;

    public List<NumberField> numberFields;

    /// <summary>
    /// Make sure to only have one instance of this sudoku grid
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // hide the message window
        msgWindows.gameObject.SetActive(false);
        // create a grid with numbers
        FillGridBase(ref solvedGrid);
        SolveGrid(ref solvedGrid);
        DebugGrid(ref solvedGrid);
        // create a riddle grid to solve
        CreateRiddleGrid(ref solvedGrid, ref riddleGrid);
        // make the grid as buttons so it can be filled
        CreateButtons();
    }

    /// <summary>
    /// Print the grid in the console for debug purposes
    /// </summary>
    /// <param name="grid">Grid we want to see</param>
    void DebugGrid(ref int[,] grid)
    {
        debugLog = "";
        for (int i = 0; i < 9; i++)
        {
            debugLog += "|";
            for (int j = 0; j < 9; j++)
            {
                debugLog += grid[i, j].ToString();
                if (j % 3 == 2)
                    debugLog += "|";
            }
            if (i % 3 == 2)
                debugLog += "\n";
            debugLog += "\n";
        }
        Debug.Log(debugLog);
    }

    /// <summary>
    /// Create buttons in the grid so it will be accessible
    /// </summary>
    void CreateButtons()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                // create button
                GameObject newButton = Instantiate(buttonPrefab);
                string id = "";

                // parenting into corresponding fields
                if (i < 3 && j < 3)
                {
                    newButton.transform.SetParent(A1, false);
                    switch (i % 3 + 1)
                    {
                        case 1:
                            id = "A";
                            break;
                        case 2:
                            id = "B";
                            break;
                        case 3:
                            id = "C";
                            break;
                    }
                    id += j % 3 + 1;

                }
                else if (i < 3 && j < 6)
                {
                    newButton.transform.SetParent(A2, false);
                    switch (i % 3 + 1)
                    {
                        case 1:
                            id = "A";
                            break;
                        case 2:
                            id = "B";
                            break;
                        case 3:
                            id = "C";
                            break;
                    }
                    id += j % 6 + 1;
                }
                else if (i < 3 && j > 5)
                {
                    newButton.transform.SetParent(A3, false);
                    switch (i % 3 + 1)
                    {
                        case 1:
                            id = "A";
                            break;
                        case 2:
                            id = "B";
                            break;
                        case 3:
                            id = "C";
                            break;
                    }
                    id += j % 9 + 1;
                }
                else if (i < 6 && j < 3)
                {
                    newButton.transform.SetParent(B1, false);
                    switch (i % 6 + 1)
                    {
                        case 4:
                            id = "D";
                            break;
                        case 5:
                            id = "E";
                            break;
                        case 6:
                            id = "F";
                            break;
                    }
                    id += j % 3 + 1;
                }
                else if (i < 6 && j < 6)
                {
                    newButton.transform.SetParent(B2, false);
                    switch (i % 6 + 1)
                    {
                        case 4:
                            id = "D";
                            break;
                        case 5:
                            id = "E";
                            break;
                        case 6:
                            id = "F";
                            break;
                    }
                    id += j % 6 + 1;
                }
                else if (i < 6 && j > 5)
                {
                    newButton.transform.SetParent(B3, false);
                    switch (i % 6 + 1)
                    {
                        case 4:
                            id = "D";
                            break;
                        case 5:
                            id = "E";
                            break;
                        case 6:
                            id = "F";
                            break;
                    }
                    id += j % 9 + 1;
                }
                else if (i > 5 && j < 3)
                {
                    newButton.transform.SetParent(C1, false);
                    switch (i % 9 + 1)
                    {
                        case 7:
                            id = "G";
                            break;
                        case 8:
                            id = "H";
                            break;
                        case 9:
                            id = "I";
                            break;
                    }
                    id += j % 3 + 1;
                }
                else if (i > 5 && j < 6)
                {
                    newButton.transform.SetParent(C2, false);
                    switch (i % 9 + 1)
                    {
                        case 7:
                            id = "G";
                            break;
                        case 8:
                            id = "H";
                            break;
                        case 9:
                            id = "I";
                            break;
                    }
                    id += j % 6 + 1;
                }
                else if (i > 5 && j > 5)
                {
                    newButton.transform.SetParent(C3, false);
                    switch (i % 9 + 1)
                    {
                        case 7:
                            id = "G";
                            break;
                        case 8:
                            id = "H";
                            break;
                        case 9:
                            id = "I";
                            break;
                    }
                    id += j % 9 + 1;
                }

                // name the button with block name and index
                newButton.name = id;

                // set values to the button
                NumberField numberField = newButton.GetComponent<NumberField>();
                numberField.SetValues(i, j, riddleGrid[i, j], id, this);
                this.numberFields.Add(numberField);
            }
        }
    }

    /// <summary>
    /// Set the input from input field into the grid
    /// </summary>
    /// <param name="x">X Coordinate (Row)</param>
    /// <param name="y">Y Coordinate (Column)</param>
    /// <param name="value">Value of the input</param>
    public void SetInputInRiddleGrid(int x, int y, int value)
    {
        riddleGrid[x, y] = value;
    }

    /// <summary>
    /// Check if the grid is completed,
    /// when the submit button is clicked
    /// </summary>
    public void CheckComplete()
    {
        if(CheckIfWon())
        {
            // show "you won" in message window in green
            msgWindows.GetComponentInChildren<TextMeshProUGUI>().text = "YOU WON!";
            msgWindows.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        }
        else
        {
            // show "try again" in message window in red
            msgWindows.GetComponentInChildren<TextMeshProUGUI>().text = "TRY AGAIN!";
            msgWindows.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }

        // show message window
        msgWindows.gameObject.SetActive(true);
        // hide everything else
        this.gameObject.SetActive(false);
        gridLabel.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        submitBtn.gameObject.SetActive(false);
        hintBtn.gameObject.SetActive(false);
        noHints.gameObject.SetActive(false);
    }

    /// <summary>
    /// Check if the riddle grid is the same as the completed grid
    /// </summary>
    /// <returns>TRUE if riddle grid is correct and completed</returns>
    bool CheckIfWon()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (riddleGrid[i, j] != solvedGrid[i, j]) // not the same
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Show hint when hint button is clicked
    /// </summary>
    public void ShowHint(ref NumberField field)
    {
        riddleGrid[field.GetX(), field.GetY()] =
                solvedGrid[field.GetX(), field.GetY()];

        field.SetHint(solvedGrid[field.GetX(), field.GetY()]);
    }

    #region all checks
    /// <summary>
    /// Check if the column already contains a number
    /// </summary>
    /// <param name="y">Y Coordinate of the column</param>
    /// <param name="value">Value that needs to be checked</param>
    /// <param name="grid">reference to the grid</param>
    /// <returns>TRUE if the value is found</returns>
    bool ColumnContainsNumber(int y, int value, ref int[,] grid)
    {
        for (int x = 0; x < 9; x++)
        {
            if(grid[x,y] == value)
            {
                // value found inside
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check if the row already contains a number
    /// </summary>
    /// <param name="x">X Coordinate of the row</param>
    /// <param name="value">Value that needs to be checked</param>
    /// <param name="grid">reference to the grid</param>
    /// <returns>TRUE if the value is found</returns>
    bool RowContainsNumber(int x, int value, ref int[,] grid)
    {
        for (int y = 0; y < 9; y++)
        {
            if(grid[x,y] == value)
            {
                // value found inside
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check if the block already contains a number
    /// </summary>
    /// <param name="x">X Coordinate of the row</param>
    /// <param name="y">Y Coordinate of the column</param>
    /// <param name="value">Value that needs to be checked</param>
    /// <param name="grid">reference to the grid</param>
    /// <returns>TRUE if the value is found</returns>
    bool BlockContainsNumber(int x, int y, int value, ref int[,] grid)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(grid[x-(x%3)+i,y-(y%3)+j] == value)
                {
                    // value found inside
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Check if the value is not in the row, column and block
    /// </summary>
    /// <param name="x">X Coordinate of the row</param>
    /// <param name="y">Y Coordinate of the column</param>
    /// <param name="value">Value that needs to be checked</param>
    /// <param name="grid">reference to the grid</param>
    /// <returns>TRUE if the value is not in the row, column and block</returns>
    bool CheckAll(int x, int y, int value, ref int[,] grid)
    {
        if (ColumnContainsNumber(y, value, ref grid)) return false;
        if (RowContainsNumber(x, value, ref grid)) return false;
        if (BlockContainsNumber(x, y, value, ref grid)) return false;
        return true; // we can put the value in the grid
    }

    /// <summary>
    /// Check if the grid is valid
    /// </summary>
    /// <param name="grid">Reference to the grid</param>
    /// <returns>TRUE if the grid is valid (no 0)</returns>
    bool GridIsValid(ref int[,] grid)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (grid[x, y] == 0)
                    return false; // not a valid grid
            }
        }
        return true;
    }
    #endregion

    #region Generate Grid
    /// <summary>
    /// Generate a grid with numbers only on first row and column
    /// </summary>
    /// <param name="grid">reference to the grid</param>
    void FillGridBase(ref int[,] grid)
    {
        List<int> rowValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> colValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        int value = rowValues[Random.Range(0, rowValues.Count)];
        grid[0, 0] = value;
        rowValues.Remove(value);
        colValues.Remove(value);
        // fill row
        for (int i = 1; i < 9; i++)
        {
            value = rowValues[Random.Range(0, rowValues.Count)];
            grid[i, 0] = value;
            rowValues.Remove(value);
        }
        // fill column
        for (int i = 1; i < 9; i++)
        {
            value = colValues[Random.Range(0, colValues.Count)];
            if(i < 3) // first block
            {
                while(BlockContainsNumber(0,0,value, ref grid))
                {
                    // reroll number
                    value = colValues[Random.Range(0, colValues.Count)];
                }
            }
            grid[0, i] = value;
            colValues.Remove(value);
        }
    }

    /// <summary>
    /// Fill the grid completely
    /// </summary>
    /// <param name="grid">reference to the grid</param>
    /// <returns>TRUE if grid is complete and valid</returns>
    bool SolveGrid(ref int[,] grid)
    {
        if(GridIsValid(ref grid))
        {
            return true;
        }

        // find first free cell
        int x = 0;
        int y = 0;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if(grid[i,j] == 0)
                {
                    // save x and y coordinates
                    x = i;
                    y = j;
                    break;
                }
            }
        }

        List<int> possibleNumbers = new List<int>();
        possibleNumbers = GetAllPossibleNumbers(x, y, ref grid);

        for (int i = 0; i < possibleNumbers.Count; i++)
        {
            // set a possible value
            grid[x, y] = possibleNumbers[i];
            // backtrack
            if (SolveGrid(ref grid))
                return true;

            // reset the cell
            grid[x, y] = 0;
        }

        return false;
    }

    /// <summary>
    /// Get all of the possible numbers for the empty grid
    /// </summary>
    /// <param name="x">X coordinate of the row</param>
    /// <param name="y">y coordinate of the column</param>
    /// <param name="grid">reference to the grid</param>
    /// <returns>A list of all possible numbers</returns>
    List<int> GetAllPossibleNumbers(int x, int y, ref int[,] grid)
    {
        List<int> possibilities = new List<int>();
        for (int val = 1; val <= 9; val++)
        {
            if(CheckAll(x, y, val, ref grid))
            {
                // value is not in column, row or block,
                // so a possible number
                possibilities.Add(val);
            }
        }
        return possibilities;
    }
    #endregion

    #region new gameplay
    /// <summary>
    /// Generate a riddle grid with 0s to play with
    /// </summary>
    /// <param name="solvedGrid">reference to the complete grid</param>
    /// <param name="riddleGrid">reference to the riddle grid</param>
    void CreateRiddleGrid(ref int[,] solvedGrid, ref int[,] riddleGrid)
    {
        // Copy the solved grid
        System.Array.Copy(solvedGrid, riddleGrid, solvedGrid.Length);

        // Erase from riddle grid, easy mode 35 pieces
        for (int i = 0; i < 35; i++)
        {
            int x1 = Random.Range(0, 9);
            int y1 = Random.Range(0, 9);

            // reroll until we find one without a 0
            while (riddleGrid[x1, y1] == 0)
            {
                x1 = Random.Range(0, 9);
                y1 = Random.Range(0, 9);
            }

            // one without 0 is found
            riddleGrid[x1, y1] = 0;
        }

        DebugGrid(ref riddleGrid);
    }
    #endregion
}
