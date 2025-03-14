using UnityEngine;

public class Dialogue_Trigger_NPC : MonoBehaviour {
    private InputActions input;

    public int timesInteractedWith;
    public bool playerInRange;

    [Header("Ink JSONs")] public TextAsset[] InkJSONs;

    void Start()
    {
        input = GetComponent<InputActions>();
    }

    void Update()
    {
        if (playerInRange && input.interactBegin)
        {
            if (timesInteractedWith < InkJSONs.Length)
            {
                DialogueManager.GetInstance().EnterDialogueMode(InkJSONs[timesInteractedWith]);
                timesInteractedWith++;
            }
            else if (timesInteractedWith == InkJSONs.Length)
            {
                DialogueManager.GetInstance().EnterDialogueMode(InkJSONs[^1]); //^1 means array length -1
            }
            else
            {
                Debug.LogError("Error related to Ink JSON files attached to the NPC.");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
    }
}