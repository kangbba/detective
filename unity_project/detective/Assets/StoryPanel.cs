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

    private string currentStory;
    private string[] sentences;
    private int currentSentenceIndex = 0;
    private Coroutine displayCoroutine;
    private bool canProceed = false;

    // Variables for testing
    public Sprite testBackSpr;
    public string testStoryStr;

    // Start is called before the first frame update
    void Start()
    {
        DisplayStory(testBackSpr, testStoryStr);
    }

    // Display the story text
    public void DisplayStory(Sprite background, string story)
    {
        storyBackgroundImg.sprite = background;
        sentences = story.Split('.');
        currentSentenceIndex = 0;
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayTextCoroutine());
    }

    // Coroutine to display text gradually
    private IEnumerator DisplayTextCoroutine()
    {
        storyText.text = "";
        canProceed = false;
        for (int i = 0; i < sentences[currentSentenceIndex].Length; i++)
        {
            storyText.text += sentences[currentSentenceIndex][i];
            yield return new WaitForSeconds(textSpeed);
        }
        canProceed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canProceed && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Advance to the next sentence or do whatever action you want
            Debug.Log("Mouse clicked, advancing to next sentence");
            canProceed = false;
            currentSentenceIndex++;
            if (currentSentenceIndex < sentences.Length)
            {
                displayCoroutine = StartCoroutine(DisplayTextCoroutine());
            }
            else
            {
                // All sentences displayed, do something else (e.g., load the next scene)
                Debug.Log("All sentences displayed");
            }
        }
    }
}
