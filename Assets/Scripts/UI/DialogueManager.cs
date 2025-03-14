using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
    // This script will:
    // Store the Ink JSON file
    // Keep track of player distance
    // Trigger dialogue manager to play

    private InputActions input;
    private static DialogueManager instance;
    private Story _currentStory;

    // Story
    private Story currentStory;

    //Tags
    private const string speakerTag = "speaker";
    private const string portraitTag = "portrait";
    private const string layoutTag = "layout";

    //Typing effect
    private float showNextCharacterAt;
    private string line;

    [Header("Booleans")] 
    public bool dialogueIsPlaying;
    public bool typing;

    [Header("Word Speed")] 
    public float wordSpeed;

    [Header("Dialogue UI")] 
    public GameObject dialogueHolder;
    public GameObject continueSymbol;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerName;

    [Header("Animators")] 
    public Animator portraitAnimator;
    public Animator layoutAnimator;
    public Animator panelAnimator;

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        input = GetComponent<InputActions>();

        if (instance != null)
        {
            Debug.Log("Found more than one dialogue manager in the scene!");
        }

        instance = this;
    }

    private void Start()
    {
        dialogueHolder.SetActive(false);
    }

    #region Handle and Display Dialogue

    void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (typing)
        {
            Typing();
        }

        if (input.interactBegin && dialogueIsPlaying)
        {
            if (typing)
            {
                SkipDialogue();
                Debug.Log("Skipped dialogue!");
            }
            else if (!typing)
            {
                ContinueStory();
            }
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueHolder.SetActive(true);
        continueSymbol.SetActive(false);
        dialogueIsPlaying = true;

        //reset tags so they don't carry over from previous story
        speakerName.text = "???";

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            currentStory.Continue();
            HandleTags(currentStory.currentTags);
            showNextCharacterAt = Time.time + wordSpeed;
            line = currentStory.currentText;
            dialogueText.text = line;
            dialogueText.maxVisibleCharacters = 0;

            typing = true;
        }

        else if (!currentStory.canContinue)
        {
            EndDialogue();
        }
    }

    private void Typing()
    {
        if (Time.time > showNextCharacterAt)
        {
            showNextCharacterAt = Time.time + wordSpeed;

            if (dialogueText.maxVisibleCharacters < line.Length)
            {
                typing = true;
                continueSymbol.SetActive(false);
                dialogueText.maxVisibleCharacters++;
            }
            else if (dialogueText.maxVisibleCharacters == line.Length)
            {
                typing = false;
                continueSymbol.SetActive(true);
            }
        }
    }

    private void SkipDialogue()
    {
        typing = false;
        continueSymbol.SetActive(true);
        dialogueText.maxVisibleCharacters = line.Length;
    }

    public void EndDialogue()
    {
        dialogueHolder.SetActive(false);
        dialogueIsPlaying = false;
    }

    #endregion

    #region Tags

    private void HandleTags(List<string> currentTags)
    {
        // loop gjennom alle tags og håndter dem
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be parsed properly: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case speakerTag:
                    speakerName.text = tagValue;
                    //panelAnimator.Play($"{tagValue}_panel");
                    break;

                case portraitTag:
                    //portraitAnimator.Play(tagValue);
                    break;
                
                case layoutTag:
                    layoutAnimator.Play(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    #endregion
}