using UnityEngine;

public class EnemyController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket ve Hasar Ayarlar�")]
    public float moveSpeed = 2f;
    public int damage = 10;
    public int maxHealth = 50;
    [SerializeField]
    private int currentHealth;

    [Header("Gezme Noktalar�")]
    public Transform[] movePoints; // D��man�n dola�aca�� noktalar
    private int currentPointIndex = 0;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Fiziksel hareketleri FixedUpdate'de ger�ekle�tiriyoruz.
    void FixedUpdate()
    {
        if (movePoints.Length > 0)
        {
            Transform target = movePoints[currentPointIndex];
            // Rigidbody'nin MovePosition metodunu kullanarak fiziksel hareket sa�lan�r.
            Vector2 newPosition = Vector2.MoveTowards(rb.position, target.position, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);

            if (Vector2.Distance(rb.position, target.position) < 0.1f)
            {
                // Sonraki hedef noktaya ge�
                currentPointIndex = (currentPointIndex + 1) % movePoints.Length;
            }

            anim.SetFloat("Speed", Mathf.Abs(moveSpeed));
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("D��man sald�r�yor!");
        // Burada oyuncu ile mesafe kontrol� yap�larak oyuncuya hasar verebilirsin.
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        anim.SetTrigger("Hurt");
        Debug.Log("D��man " + damageAmount + " hasar ald�!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetBool("IsDead", true);
        Debug.Log("D��man �ld�!");
        Destroy(gameObject, 2f);
    }
}
