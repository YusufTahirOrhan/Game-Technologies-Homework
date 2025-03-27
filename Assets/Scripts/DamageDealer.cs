using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int Damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(Damage);
            Debug.Log("DamageDealer hasar uyguladý: " + Damage);
        }
    }
}
