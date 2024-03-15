using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class StoryPanel : MonoBehaviour
{
    public Image storyBackgroundImg;
    public TextMeshProUGUI storyText;
    public float textSpeed = 0.05f;

    private string[] storySentences;
    private int currentSentenceIndex = 0;
    private Coroutine displayCoroutine;

    // Variables for testing
    public Sprite testBackSpr;
    public string testStoryStr;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStorySentences(testStoryStr);
        DisplayCurrentSentence();
    }

    // Initialize the story sentences array
    private void InitializeStorySentences(string story)
    {
        storySentences = story.Split('.');
        // Add the '.' back to each sentence
        for (int i = 0; i < storySentences.Length; i++)
        {
            storySentences[i] += ".";
        }
    }

    // Display the current sentence
    private void DisplayCurrentSentence()
    {
        if (currentSentenceIndex < storySentences.Length)
        {
            displayCoroutine = StartCoroutine(DisplayTextCoroutine(storySentences[currentSentenceIndex]));
        }
        else
        {
            // All sentences displayed, do something else (e.g., load the next scene)
            Debug.Log("All sentences displayed");
        }
    }

    // Coroutine to display text gradually
    private IEnumerator DisplayTextCoroutine(string sentence)
    {
        for (int i = 0; i < sentence.Length; i++)
        {
            storyText.text += sentence[i];
            yield return new WaitForSeconds(textSpeed);
        }
        // Wait for mouse click to proceed to the next sentence
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        currentSentenceIndex++;
        DisplayCurrentSentence();
    }
}
