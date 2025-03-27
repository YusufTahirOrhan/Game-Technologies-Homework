using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket Ayarlar�")]
    public float moveSpeed = 2f;
    public float idleWaitTime = 1.0f; // Noktada bekleme s�resi

    [Header("Sa�l�k ve Hasar Ayarlar�")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Sald�r� Ayarlar�")]
    public float attackCooldown = 1f; // Sald�r�lar aras� bekleme s�resi
    private float lastAttackTime = 0f;
    public float attackRangeDistance = 1.5f; // Oyuncuya yeterince yakla��nca sald�r� yap�lacak mesafe

    [Header("Gezme Noktalar�")]
    public Transform[] movePoints; // D��man�n dola�aca�� noktalar
    private int currentPointIndex = 0;
    private bool isWaiting = false;

    public Transform Character; // Sprite'�n y�n�n� de�i�tirmek i�in referans

    private Rigidbody2D rb;
    private Animator anim;

    // Takip modu i�in: E�er oyuncu attack range i�inde ise buraya atan�r.
    private Transform chaseTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        // E�er oyuncu takip modunda varsa
        if (chaseTarget != null)
        {
            Vector2 direction = ((Vector2)chaseTarget.position - rb.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // Karakterin y�n�n� ayarla
            if (direction.x < -0.1f)
            {
                Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            else if (direction.x > 0.1f)
            {
                Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            anim.SetBool("IsRunning", true);

            // E�er oyuncuya yeterince yak�nsa sald�r
            float dist = Vector2.Distance(rb.position, chaseTarget.position);
            if (dist <= attackRangeDistance)
            {
                Attack();
            }
        }
        else
        {
            // Takip modunda de�ilse, patrol davran���
            if (!isWaiting)
            {
                if (movePoints.Length > 0)
                {
                    Transform target = movePoints[currentPointIndex];
                    Vector2 direction = ((Vector2)target.position - rb.position).normalized;
                    rb.linearVelocity = direction * moveSpeed;

                    if (direction.x < -0.1f)
                    {
                        Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
                    }
                    else if (direction.x > 0.1f)
                    {
                        Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
                    }

                    // Noktaya ula��nca bekle
                    if (Vector2.Distance(rb.position, target.position) < 0.1f)
                    {
                        StartCoroutine(WaitAtPoint());
                    }
                    anim.SetBool("IsRunning", true);
                }
                else
                {
                    anim.SetBool("IsRunning", false);
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("IsRunning", false);
            }
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("IsRunning", false);
        yield return new WaitForSeconds(idleWaitTime);
        currentPointIndex = (currentPointIndex + 1) % movePoints.Length;
        isWaiting = false;
    }

    // Sald�r� metodunda cooldown mekanizmas�
    public void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        Debug.Log("D��man sald�r�yor!");
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
        anim.SetTrigger("Die");
        Debug.Log("D��man �ld�!");
        Destroy(gameObject, 2f);
    }

    // Bu metodlar, AttackRange collider'�na ba�l� script taraf�ndan �a�r�lacak
    public void SetChaseTarget(Transform target)
    {
        chaseTarget = target;
    }

    public void ClearChaseTarget()
    {
        chaseTarget = null;
    }
}