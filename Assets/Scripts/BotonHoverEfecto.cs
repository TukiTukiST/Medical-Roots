using UnityEngine;
using UnityEngine.EventSystems;

public class BotonHoverEfecto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Configuración de Escala")]
    [SerializeField] private Vector3 escalaNormal = Vector3.one;
    [SerializeField] private Vector3 escalaHover = new Vector3(1.1f, 1.1f, 1f);
    [SerializeField] private float velocidadTransicion = 10f;

    private Vector3 escalaObjetivo;

    private void Awake()
    {
        escalaNormal = transform.localScale;
        escalaObjetivo = escalaNormal;
    }

    private void Update()
    {
        // Interpola suavemente la escala
        transform.localScale = Vector3.Lerp(transform.localScale, escalaObjetivo, Time.unscaledDeltaTime * velocidadTransicion);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaObjetivo = escalaHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaNormal;
    }

    private void OnDisable()
    {
        // Restablece la escala por si el botón se oculta
        transform.localScale = escalaNormal;
        escalaObjetivo = escalaNormal;
    }
}