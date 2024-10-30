using System.Collections;
using UnityEngine;

public class SmoothMover : MonoBehaviour
{
    public Transform objectToMove; // Objeto que se moverá
    public Transform targetPosition; // Punto de destino del movimiento
    public float moveDuration = 2.0f; // Tiempo que tomará el movimiento en segundos
    public AudioSource moveSound; // Sonido que se reproducirá durante el movimiento
    public float delayTime = 30.0f; // Tiempo de espera antes de comenzar el movimiento en segundos

    private Vector3 startPosition; // Posición inicial del objeto
    private bool isMoving = false; // Indicador de si el objeto está moviéndose

    private void Start()
    {
        // Guarda la posición inicial del objeto que se moverá
        if (objectToMove != null)
        {
            startPosition = objectToMove.position;

            // Inicia la corrutina para activar el movimiento después del tiempo de espera
            StartCoroutine(DelayedMovement());
        }
        else
        {
            Debug.LogWarning("No se ha asignado el objeto a mover en el inspector.");
        }
    }

    private IEnumerator DelayedMovement()
    {
        // Espera el tiempo especificado antes de iniciar el movimiento
        yield return new WaitForSeconds(delayTime);

        // Llama al método para iniciar el movimiento
        StartMoving();
    }

    public void StartMoving()
    {
        // Solo comienza el movimiento si no está en proceso de movimiento
        if (!isMoving && objectToMove != null)
        {
            StartCoroutine(MoveToPosition());
        }
    }

    private IEnumerator MoveToPosition()
    {
        isMoving = true;

        // Inicia el sonido si no está sonando
        if (moveSound && !moveSound.isPlaying)
        {
            moveSound.Play();
        }

        // Movimiento suave del objeto usando Lerp
        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            // Interpolación entre la posición inicial y la posición objetivo
            objectToMove.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / moveDuration);

            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegura que el objeto está en la posición exacta del destino
        objectToMove.position = targetPosition.position;

        // Detiene el sonido
        if (moveSound && moveSound.isPlaying)
        {
            moveSound.Stop();
        }

        isMoving = false;
    }
}
