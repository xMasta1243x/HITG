using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; // El AudioSource que reproducir� los clips de m�sica
    public AudioClip initialMusicClip; // El AudioClip que se reproducir� inicialmente
    public AudioClip newMusicClip; // El AudioClip que se reproducir� despu�s de la colisi�n
    public float fadeInDuration = 3.0f; // Duraci�n del Fade In en segundos
    public float fadeOutDuration = 2.0f; // Duraci�n del Fade Out en segundos
    public float maxVolume = 1.0f; // Volumen m�ximo de la m�sica
    public float delayTime = 5.0f; // Tiempo de espera antes de iniciar la m�sica inicial

    private void Start()
    {
        if (audioSource != null && initialMusicClip != null)
        {
            // Configura el AudioClip inicial en el AudioSource y el volumen en 0
            audioSource.clip = initialMusicClip;
            audioSource.volume = 0f;

            // Comienza la m�sica inicial con Fade In despu�s de un retraso
            StartCoroutine(PlayMusicWithFadeIn(audioSource, delayTime));
        }
        else
        {
            Debug.LogWarning("Aseg�rate de asignar el AudioSource y el AudioClip inicial en el MusicManager.");
        }
    }

    private IEnumerator PlayMusicWithFadeIn(AudioSource source, float delay)
    {
        // Espera antes de iniciar el Fade In
        yield return new WaitForSeconds(delay);

        // Inicia la reproducci�n
        source.Play();

        // Realiza el Fade In para alcanzar el volumen m�ximo
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

        // Detiene la m�sica y deja el volumen en 0
        source.volume = 0f;
        source.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que colision� tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Cambia la m�sica cuando el jugador entra en el trigger
            StartCoroutine(SwitchMusic());
        }
    }

    private IEnumerator SwitchMusic()
    {
        // Fade Out de la m�sica actual
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
