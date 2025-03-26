using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning("aa");
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Debug.Log("DamageDealer hasar uyguladý: " + damage);
        }
    }
}
