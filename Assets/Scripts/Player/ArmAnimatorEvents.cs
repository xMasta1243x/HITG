using UnityEngine;

public class ArmAnimatorEvents : MonoBehaviour
{
    public Weapon weapon; // Arrastra aquí el objeto Weapon en el Inspector
    public KnifeWeapon knifeWeapon;

    // Este método se llamará desde el evento de la animación
    public void CreateBullet()
    {
        if (weapon != null)
        {
            weapon.CreateBullet(); // Llama al método en el script Weapon
        }
        else
        {
            Debug.LogError("No se ha asignado el script Weapon en ArmAnimatorEvents.");
        }
    }

    public void EnableKnifeAttack()
    {
        knifeWeapon.EnableKnifeCollider(); // Método que activa el collider en KnifeWeapon
    }

    public void DisableKnifeAttack()
    {
        knifeWeapon.DisableKnifeCollider(); // Método que desactiva el collider en KnifeWeapon
    }
}

