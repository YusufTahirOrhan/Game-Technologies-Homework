using UnityEngine;

public class HitboxController : DamageDealer
{
    public float activeTime = 0.2f; // Hitbox'ýn aktif kalma süresi

    // Animasyon eventi tarafýndan çaðrýlabilir
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
