using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; // El AudioSource que reproducirá los clips de música
    public AudioClip initialMusicClip; // El AudioClip que se reproducirá inicialmente
    public AudioClip newMusicClip; // El AudioClip que se reproducirá después de la colisión
    public float fadeInDuration = 3.0f; // Duración del Fade In en segundos
    public float fadeOutDuration = 2.0f; // Duración del Fade Out en segundos
    public float maxVolume = 1.0f; // Volumen máximo de la música
    public float delayTime = 5.0f; // Tiempo de espera antes de iniciar la música inicial

    private void Start()
    {
        if (audioSource != null && initialMusicClip != null)
        {
            // Configura el AudioClip inicial en el AudioSource y el volumen en 0
            audioSource.clip = initialMusicClip;
            audioSource.volume = 0f;

            // Comienza la música inicial con Fade In después de un retraso
            StartCoroutine(PlayMusicWithFadeIn(audioSource, delayTime));
        }
        else
        {
            Debug.LogWarning("Asegúrate de asignar el AudioSource y el AudioClip inicial en el MusicManager.");
        }
    }

    private IEnumerator PlayMusicWithFadeIn(AudioSource source, float delay)
    {
        // Espera antes de iniciar el Fade In
        yield return new WaitForSeconds(delay);

        // Inicia la reproducción
        source.Play();

        // Realiza el Fade In para alcanzar el volumen máximo
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            source.volume = Mathf.Lerp(0f, maxVolume, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        source.volume = maxVolume;
    }

    private IEnumerator FadeOutMusic(AudioSource source)
    {
        float startVolume = source.volume;
        float elapsedTime = 0f;

        // Reduce el volumen hasta 0 en el tiempo especificado
        while (elapsedTime < fadeOutDuration)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeOutDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Detiene la música y deja el volumen en 0
        source.volume = 0f;
        source.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que colisionó tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Cambia la música cuando el jugador entra en el trigger
            StartCoroutine(SwitchMusic());
        }
    }

    private IEnumerator SwitchMusic()
    {
        // Fade Out de la música actual
        if (audioSource.isPlaying)
        {
            yield return StartCoroutine(FadeOutMusic(audioSource));
        }

        // Cambia el clip al nuevo y realiza un Fade In
        if (newMusicClip != null)
        {
            audioSource.clip = newMusicClip;
            audioSource.Play();
            yield return StartCoroutine(PlayMusicWithFadeIn(audioSource, 0f));
        }
    }
}
