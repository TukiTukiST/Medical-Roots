using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("Datos del NPC")]
    [SerializeField] private string npcName = "Rosa";

    [Header("Secuencia de Diálogo")]
    [SerializeField] private DialogueNode[] dialogueSequence;

    [Header("UI Indicador")]
    [SerializeField] private GameObject interactionPrompt;

    private bool isPlayerInRange = false;

    private void Update()
    {
        // Solo inicia el diálogo si el jugador está cerca, presiona E y NO hay un diálogo corriendo ya
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueManager.Instance != null && !DialogueManager.Instance.IsDialogueActive)
            {
                if (interactionPrompt != null)
                    interactionPrompt.SetActive(false);

                DialogueManager.Instance.StartDialogue(npcName, dialogueSequence);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null && (DialogueManager.Instance == null || !DialogueManager.Instance.IsDialogueActive))
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
        }
    }
}