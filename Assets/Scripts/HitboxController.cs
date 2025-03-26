using UnityEngine;

public class HitboxController : DamageDealer
{
    public float activeTime = 0.2f; // Hitbox'�n aktif kalma s�resi

    // Animasyon eventi taraf�ndan �a�r�labilir
    public void ActivateHitbox()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true;
            Invoke(nameof(DeactivateHitbox), activeTime);
        }
    }

    private void DeactivateHitbox()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
}
