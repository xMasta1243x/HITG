using UnityEngine;

public class SwitchWeapons : MonoBehaviour
{
    public GameObject[] weapons; // Armas que el jugador puede usar
    public Animator animator; // Referencia al Animator
    private int currentWeaponIndex = 0;

    void Start()
    {
        // Comienza el juego con la primera arma activa
        SelectWeapon(currentWeaponIndex);
    }

    void Update()
    {
        // Verifica si el jugador est� atacando, disparando o recargando
        bool isAttacking = animator.GetBool("IsAttacking");
        bool isShooting = animator.GetBool("IsShooting");
        bool isReloading = animator.GetBool("IsReloading");

        // Solo permite cambiar de armas si no se est� atacando, disparando o recargando
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking && !isShooting && !isReloading)
        {
            SwitchToNextWeapon();
        }
    }

    private void SwitchToNextWeapon()
    {
        // Desactiva la arma actual
        weapons[currentWeaponIndex].SetActive(false);

        // Cambia el �ndice al siguiente arma
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

        // Selecciona la nueva arma
        SelectWeapon(currentWeaponIndex);
    }

    private void SelectWeapon(int index)
    {
        // Activa la nueva arma y desactiva las dem�s
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        // Actualiza el par�metro "WeaponType" en el Animator para la capa correcta
        animator.SetInteger("WeaponType", index);

        // Resetea los par�metros de ataque, recarga y disparo al cambiar de arma
        ResetAttackStates();

        // Asigna el peso de la capa seg�n el arma seleccionada
        for (int i = 1; i < animator.layerCount; i++) // Comienza en 1 para ignorar Base Layer
        {
            animator.SetLayerWeight(i, i - 1 == index ? 1 : 0); // Ajustar el �ndice para que coincida con las capas de armas
        }
    }

    private void ResetAttackStates()
    {
        // Resetea todos los estados de ataque al cambiar de arma
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsReloading", false);
        animator.SetBool("IsShooting", false);
    }
}



