using UnityEngine;
using TMPro;

public class MunaPlant : MonoBehaviour
{
    [Header("Datos de la Planta")]
    [SerializeField] private string plantName = "Muña";
    [SerializeField] private int quantity = 1;

    [Header("Indicador de Interacción")]
    [SerializeField] private GameObject interactionPrompt; // UI "E Recolectar" sobre la planta

    [Header("Efectos Opcionales")]
    [SerializeField] private GameObject collectEffectPrefab; // Partículas o destello al recoger
    [SerializeField] private AudioClip collectSound; // Sonido de recolección

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // 1. Opcional: Reproducir sonido de recolección
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // 2. Opcional: Instanciar efecto visual/partículas
        if (collectEffectPrefab != null)
        {
            Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
        }

        Debug.Log($"¡Has recolectado {quantity}x {plantName}!");

        // Aquí conectarás con tu script de Inventario o Gestor de Misiones más adelante
        // Ejemplo: InventoryManager.Instance.AddItem(plantName, quantity);

        // 3. Desactivar el indicador y destruir el objeto de la escena
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (interactionPrompt != null)
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