using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de Transición")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private string gameSceneName = "Juego"; // Cambia este nombre por el de tu escena de juego

    public void OnPlayButtonPressed()
    {
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        // Bloqueamos la interacción para evitar clics dobles mientras se oscurece
        fadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }

        // Nos aseguramos de que quede 100% negro antes de cambiar de escena
        fadeCanvasGroup.alpha = 1f;

        // Carga la escena del juego
        SceneManager.LoadScene(gameSceneName);
    }
}