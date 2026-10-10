using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("Datos del NPC")]
    [SerializeField] private string npcName = "Rosa";

    [Header("Referencias de UI (En Rosa)")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Secuencia de Diálogo")]
    [SerializeField] private DialogueNode[] dialogueSequence;

    [Header("UI Indicador")]
    [SerializeField] private GameObject interactionPrompt;

    [Header("Efecto de Texto")]
    [SerializeField] private float typingSpeed = 0.02f;

    private bool isPlayerInRange = false;
    private int currentSentenceIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private void Start()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Si el cuadro de diálogo aún no está abierto, lo abrimos e iniciamos
            if (dialogueCanvas != null && !dialogueCanvas.activeSelf)
            {
                StartDialogue();
            }
            else
            {
                // Si ya está abierto, pasamos a la siguiente frase o completamos la actual
                DisplayNextSentence();
            }
        }
    }

    private void StartDialogue()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(true);

        if (nameText != null)
            nameText.text = npcName;

        currentSentenceIndex = 0;
        ShowCurrentNode();
    }

    private void DisplayNextSentence()
    {
        // Si el texto aún se está escribiendo letra por letra, lo completa inmediatamente al presionar E
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueSequence[currentSentenceIndex].sentence;
            isTyping = false;
            return;
        }

        // Avanzar a la siguiente frase
        currentSentenceIndex++;

        if (currentSentenceIndex < dialogueSequence.Length)
        {
            ShowCurrentNode();
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowCurrentNode()
    {
        DialogueNode currentNode = dialogueSequence[currentSentenceIndex];

        if (portraitImage != null && currentNode.portrait != null)
        {
            portraitImage.sprite = currentNode.portrait;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(currentNode.sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void EndDialogue()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        // Si Ari sigue cerca, vuelve a mostrar la llamada de acción "E Hablar"
        if (isPlayerInRange && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (interactionPrompt != null && (dialogueCanvas == null || !dialogueCanvas.activeSelf))
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(false);
        }
    }
}