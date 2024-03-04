using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    private GameObject dialogueBox;
    private TextMeshProUGUI dialogueText;
    private string[] sentences;
    private int index;
    private bool isWriting;
    public static bool dialogueActive;

    private BoxCollider2D bc;

    private bool hasBeenTriggered = false;

    void Start(){
        bc = GetComponent<BoxCollider2D>();
        dialogueBox = GameObject.Find("DialogueBox");
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();
        dialogueBox.SetActive(false);
    }

    void triggerDialogue(){
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        dialogueActive = true;
        hasBeenTriggered = true;
        dialogueBox.SetActive(true);
        sentences = dialogue.sentences;
        dialogueText.text = "";
        isWriting = true;
        for (int i = 0; i < sentences[0].Length; i++)
        {
            char letter = sentences[0][i];

            if (letter == '<')
            {
                // skip the tag
                ArrayList tag = new ArrayList();
                while (letter != '>')
                {
                    tag.Add(letter);
                    yield return new WaitForSeconds(dialogue.writeSpeed);
                    letter = sentences[0][++i];
                }
                // convert tag to string and add to dialogueText
                string tagString = new string((char[])tag.ToArray(typeof(char)));
                dialogueText.text += tagString;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogue.writeSpeed);

            if (letter == '.' || letter == '!' || letter == '?')
            {
                yield return new WaitForSeconds(0.5f);
            }
            else if (letter == ',')
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
        isWriting = false;
    }

    IEnumerator NextSentence(){
        if(index < sentences.Length - 1){
            index++;
            dialogueText.text = "";
            isWriting = true;
            for (int i = 0; i < sentences[index].Length; i++)
            {
                char letter = sentences[index][i];

                if (letter == '<')
                {
                    // skip the tag
                    ArrayList tag = new ArrayList();
                    while (letter != '>')
                    {
                        tag.Add(letter);
                        yield return new WaitForSeconds(dialogue.writeSpeed);
                        letter = sentences[index][++i];
                    }
                    string tagString = new string((char[])tag.ToArray(typeof(char)));
                    dialogueText.text += tagString;
                }

                dialogueText.text += letter;
                yield return new WaitForSeconds(dialogue.writeSpeed);

                if (letter == '.' || letter == '!' || letter == '?')
                {
                    yield return new WaitForSeconds(0.5f);
                }
                else if (letter == ',')
                {
                    yield return new WaitForSeconds(0.2f);
                }
            }
            isWriting = false;
        } else {
            dialogueBox.SetActive(false);
            index = 0;

            yield return new WaitForSeconds(0.5f);
            dialogueActive = false;
        }
    }

    void interruptDialogue(){
        if(isWriting){
            StopAllCoroutines();
            dialogueText.text = sentences[index];
            isWriting = false;
        } else {
            StartCoroutine(NextSentence());
        }
    }

    void Update(){
        // check for collision with player
        if(bc.IsTouchingLayers(LayerMask.GetMask("Player")) && !dialogueBox.activeSelf && !hasBeenTriggered){
            StartCoroutine(StartDialogue());
        }

        if(Input.GetMouseButtonDown(0)){
            if(dialogueBox.activeSelf){
                interruptDialogue();
            }
        }
    }
}
