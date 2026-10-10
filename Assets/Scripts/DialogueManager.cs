using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueNode
{
    public Sprite portrait;
    [TextArea(2, 4)]
    public string sentence;
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image portraitImage;

    [Header("Configuración")]
    [SerializeField] private float typingSpeed = 0.02f;

    private Queue<DialogueNode> nodesQueue = new Queue<DialogueNode>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string currentSentence;
    
    // Control de estado del diálogo activo
    public bool IsDialogueActive { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // Si el diálogo está activo y el jugador presiona E, Espacio o Clic, avanza la frase
        if (IsDialogueActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(string speakerName, DialogueNode[] nodes)
    {
        if (nodes == null || nodes.Length == 0) return;

        IsDialogueActive = true;
        
        // Forzamos la activación del panel
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (nameText != null)
            nameText.text = speakerName;

        nodesQueue.Clear();
        foreach (DialogueNode node in nodes)
        {
            nodesQueue.Enqueue(node);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // Si aún está escribiendo el texto, lo muestra completo de golpe al presionar la tecla
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        // Si ya no quedan más frases, cierra el diálogo
        if (nodesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueNode currentNode = nodesQueue.Dequeue();

        if (portraitImage != null && currentNode.portrait != null)
        {
            portraitImage.sprite = currentNode.portrait;
        }

        currentSentence = currentNode.sentence;
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
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

    public void EndDialogue()
    {
        IsDialogueActive = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }
}