using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KnifeWeapon : MonoBehaviour
{
    public Animator animator;        // Referencia al Animator
    public Collider knifeCollider;   // Referencia al collider del cuchillo
    public float attackCooldown = 1f; // Tiempo de espera entre ataques

    private bool canAttack = true;

    void Update()
    {
        // Si se presiona el botón de ataque (ej. botón izquierdo del ratón) y está listo para atacar
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false; // Desactivar posibilidad de atacar hasta que el cooldown termine

        // Activar el parámetro "IsAttacking" en el Animator para reproducir la animación
        animator.SetBool("IsAttacking", true);

        // Activar el collider aquí si quieres que esté activo durante el ataque
        EnableKnifeCollider();

        // Iniciar el cooldown de ataque
        StartCoroutine(ResetAttackCooldown());
    }

    // Método para activar el collider de ataque
    public void EnableKnifeCollider()
    {
        knifeCollider.enabled = true; // Activa el collider
    }

    // Método para desactivar el collider de ataque
    public void DisableKnifeCollider()
    {
        knifeCollider.enabled = false; // Desactiva el collider
    }

    // Cooldown para volver a habilitar el ataque
    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        animator.SetBool("IsAttacking", false); // Termina la animación de ataque
    }

    // Detecta colisiones con el collider del cuchillo
    private void OnTriggerEnter(Collider other)
    {
        // Aquí puedes agregar la lógica para hacer daño a los objetos con los que colisiona
        // Ejemplo:
        // if (other.CompareTag("Enemy"))
        // {
        //     other.GetComponent<Enemy>().TakeDamage(knifeDamage);
        // }
    }
}

