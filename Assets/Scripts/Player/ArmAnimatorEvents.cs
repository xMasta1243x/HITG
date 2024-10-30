using UnityEngine;

public class ArmAnimatorEvents : MonoBehaviour
{
    public Weapon weapon; // Arrastra aqu� el objeto Weapon en el Inspector
    public KnifeWeapon knifeWeapon;

    // Este m�todo se llamar� desde el evento de la animaci�n
    public void CreateBullet()
    {
        if (weapon != null)
        {
            weapon.CreateBullet(); // Llama al m�todo en el script Weapon
        }
        else
        {
            Debug.LogError("No se ha asignado el script Weapon en ArmAnimatorEvents.");
        }
    }

    public void EnableKnifeAttack()
    {
        knifeWeapon.EnableKnifeCollider(); // M�todo que activa el collider en KnifeWeapon
    }

    public void DisableKnifeAttack()
    {
        knifeWeapon.DisableKnifeCollider(); // M�todo que desactiva el collider en KnifeWeapon
    }
}

