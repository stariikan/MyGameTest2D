using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
{
    public Text textUI;
    public string[] textArray;
    public float speed = 0.1f;
    public float speedDel = 0.1f;
    public int maxLineLength = 150;
    public int maxLines = 12;

    private int currentLineIndex = 0;
    private int currentLine = 0;
    private int linesDisplayed = 0;
    private int storyNum;
    private float timer = 0.0f;
    private float delTimer = 0.0f;
    private string currentText = "";

    void Start()
    {
        // Set the initial text to an empty string
        textUI.text = "";
        storyNum = HeroText.Instance.storyNum;
    }

    void Update()
    {
        // If we've displayed all the lines in the array, do nothing
        if (currentLineIndex >= textArray.Length)
        {
            return;
        }

        // If the current text is empty, select a new random text from the array
        if (currentText.Length == 0)
        {
            currentText = textArray[storyNum];
        }

        // If we haven't finished displaying the current line, keep displaying it
        if (currentLine < currentText.Length)
        {
            timer += Time.deltaTime;
            delTimer += Time.deltaTime;
            if (timer >= speed)
            {
                timer = 0.0f;
                currentLine++;
                textUI.text = currentText.Substring(0, currentLine);

                // If the current line exceeds the maximum length, remove the first characters
                if (currentLine > maxLineLength && delTimer >= speedDel)
                {
                    delTimer = 0.0f;
                    textUI.text = textUI.text.Substring(currentLine - maxLineLength);
                }

                 //If the current text exceeds the maximum number of lines, roll it up
                if (textUI.cachedTextGenerator.lineCount > maxLines)
                {
                    // Get the index of the end of the first line
                    int endOfFirstLineIndex = textUI.cachedTextGenerator.lines[0].startCharIdx;
                    // Remove the first line
                    currentText = currentText.Substring(endOfFirstLineIndex);
                    // Reset the current line index to the length of the removed line
                    currentLine -= endOfFirstLineIndex;
                    // Increase the lines displayed counter
                    linesDisplayed++;
                }
            }
        }
        // If we've finished displaying the current line, move on to the next one
        //else
        //{
        //    currentLineIndex++;
         //   currentLine = 0;
         //   linesDisplayed = 0;
         //   currentText = "";

        //    if (currentLineIndex < textArray.Length)
        //    {
              //Split the text into separate lines based on the text box size
         //       textUI.text = "";
        //    }
       // }
    }
    public void ChangeTextSpeed()
    {
       speed = 0.001f;
    }
}
