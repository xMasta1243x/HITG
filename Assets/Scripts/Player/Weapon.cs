using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public float fireDelay = 2f; // Tiempo de espera entre disparos

    public GameObject armObject; // Asigna aquí el objeto brazo en el Inspector
    private Animator animator;   // Referencia al Animator en el brazo

    private bool isShooting; // Variable para controlar el estado de disparo
    private bool canFire = true; // Variable para controlar si se puede disparar
    private bool isReloading; // Variable para controlar si se está recargando

    private int currentAmmo; // Balas actuales
    private int maxAmmo = 7; // Máximo de balas

    // Agregar referencias para los sonidos
    public AudioClip shootSound; // Sonido de disparo
    public AudioClip reloadSound; // Sonido de recarga
    private AudioSource audioSource; // Componente AudioSource

    void Start()
    {
        currentAmmo = maxAmmo; // Inicializa la munición
        if (armObject != null)
        {
            animator = armObject.GetComponent<Animator>(); // Obtén el Animator del brazo
        }
        else
        {
            Debug.LogError("No se ha asignado el objeto brazo en el script Weapon.");
        }

        // Obtén el componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Verifica si se puede recargar
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload()); // Inicia la recarga si no se está recargando y si no está lleno
        }

        // Verifica si se está disparando
        if (Input.GetKeyDown(KeyCode.Mouse0) && canFire && !isReloading && currentAmmo > 0)
        {
            isShooting = true; // Activa el estado de disparo
            CreateBullet(); // Dispara la bala
            currentAmmo--; // Reduce la munición
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isShooting = false; // Desactiva el estado de disparo
        }

        // Actualiza los parámetros del Animator
        animator.SetBool("IsShooting", isShooting);
        animator.SetBool("IsReloading", isReloading);
    }

    // Este método se llamará desde el evento de la animación
    public void CreateBullet()
    {
        if (!canFire) return; // Si no se puede disparar, salir

        canFire = false; // Desactiva la capacidad de disparar

        // Ajusta la rotación de la bala
        Quaternion rotationAdjustment = Quaternion.Euler(90, 0, 0); // Ajusta el valor según necesites
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation * rotationAdjustment);

        // Aplica la fuerza a la bala
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
        StartCoroutine(FireCooldown()); // Inicia la corutina de cooldown

        // Reproduce el sonido de disparo
        PlaySound(shootSound);
    }

    private IEnumerator Reload()
    {
        if (currentAmmo >= maxAmmo) yield break; // Si ya tiene el máximo de balas, no recarga

        isReloading = true; // Marca que se está recargando
        animator.SetBool("IsReloading", true); // Inicia la animación de recarga

        // Espera el tiempo que dure la animación de recarga (ajusta el tiempo según necesites)
        yield return new WaitForSeconds(1.167f); // Ajusta este tiempo al que dure la animación

        currentAmmo = maxAmmo; // Rellena las balas
        isReloading = false; // Marca que la recarga ha terminado
        animator.SetBool("IsReloading", false); // Finaliza la animación de recarga

        // Reproduce el sonido de recarga
        PlaySound(reloadSound);
    }

    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(fireDelay); // Espera el tiempo de delay
        canFire = true; // Permite volver a disparar
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    // Método para reproducir sonido
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Sonido o AudioSource no asignados.");
        }
    }
}





