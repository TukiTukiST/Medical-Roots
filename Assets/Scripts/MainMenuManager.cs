using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [Tooltip("Nombre exacto de la escena del juego en Build Settings/Profiles")]
    [SerializeField] private string nombreEscenaJuego = "Nivel_1";

    [Header("Paneles de Menú (Opcional)")]
    [SerializeField] private GameObject panelAjustes;
    [SerializeField] private GameObject panelCreditos;

    [Header("Efectos de Sonido (UI)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoClick;
    [SerializeField] private AudioClip sonidoHover;

    // --- MÉTODOS DE LOS BOTONES ---

    public void BotonJugar()
    {
        StartCoroutine(CargarEscenaConSonido());
    }

    public void BotonAjustes()
    {
        ReproducirSonido(sonidoClick);
        if (panelAjustes != null)
        {
            panelAjustes.SetActive(!panelAjustes.activeSelf);
            // Si abrimos Ajustes, aseguramos cerrar Créditos
            if (panelCreditos != null && panelAjustes.activeSelf) 
                panelCreditos.SetActive(false);
        }
    }

    public void BotonCreditos()
    {
        ReproducirSonido(sonidoClick);
        if (panelCreditos != null)
        {
            panelCreditos.SetActive(!panelCreditos.activeSelf);
            // Si abrimos Créditos, aseguramos cerrar Ajustes
            if (panelAjustes != null && panelCreditos.activeSelf) 
                panelAjustes.SetActive(false);
        }
    }

    public void BotonSalir()
    {
        ReproducirSonido(sonidoClick);
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    // --- MÉTODOS AUXILIARES ---

    public void ReproducirSonidoClick()
    {
        ReproducirSonido(sonidoClick);
    }

    public void ReproducirSonidoHover()
    {
        ReproducirSonido(sonidoHover);
    }

    private void ReproducirSonido(AudioClip clip)
    {
        // Usa directamente el AudioSource asignado en 2D sin depender de la Cámara
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Espera a que termine la duración completa del clip (o la que definas) antes de cambiar de escena
    private IEnumerator CargarEscenaConSonido()
    {
        ReproducirSonido(sonidoClick);
        
        if (sonidoClick != null)
        {
            // Espera el tiempo exacto que dura tu clip de audio para que no se corte
            yield return new WaitForSecondsRealtime(sonidoClick.length);
        }

        SceneManager.LoadScene(nombreEscenaJuego);
    }
}