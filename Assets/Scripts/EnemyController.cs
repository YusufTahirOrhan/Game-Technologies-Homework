using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 2f;
    public float idleWaitTime = 1.0f; // Noktada bekleme süresi

    [Header("Saðlýk ve Hasar Ayarlarý")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("Saldýrý Ayarlarý")]
    public float attackCooldown = 1f; // Saldýrýlar arasý bekleme süresi
    private float lastAttackTime = 0f;
    public float attackRangeDistance = 1.5f; // Oyuncuya yeterince yaklaþýnca saldýrý yapýlacak mesafe

    [Header("Gezme Noktalarý")]
    public Transform[] movePoints; // Düþmanýn dolaþacaðý noktalar
    private int currentPointIndex = 0;
    private bool isWaiting = false;

    public Transform Character; // Sprite'ýn yönünü deðiþtirmek için referans

    private Rigidbody2D rb;
    private Animator anim;

    // Takip modu için: Eðer oyuncu attack range içinde ise buraya atanýr.
    private Transform chaseTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        // Eðer oyuncu takip modunda varsa
        if (chaseTarget != null)
        {
            Vector2 direction = ((Vector2)chaseTarget.position - rb.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // Karakterin yönünü ayarla
            if (direction.x < -0.1f)
            {
                Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            else if (direction.x > 0.1f)
            {
                Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            anim.SetBool("IsRunning", true);

            // Eðer oyuncuya yeterince yakýnsa saldýr
            float dist = Vector2.Distance(rb.position, chaseTarget.position);
            if (dist <= attackRangeDistance)
            {
                Attack();
            }
        }
        else
        {
            // Takip modunda deðilse, patrol davranýþý
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

                    // Noktaya ulaþýnca bekle
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

    // Saldýrý metodunda cooldown mekanizmasý
    public void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        Debug.Log("Düþman saldýrýyor!");
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        anim.SetTrigger("Hurt");
        Debug.Log("Düþman " + damageAmount + " hasar aldý!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Die");
        Debug.Log("Düþman öldü!");
        Destroy(gameObject, 2f);
    }

    // Bu metodlar, AttackRange collider'ýna baðlý script tarafýndan çaðrýlacak
    public void SetChaseTarget(Transform target)
    {
        chaseTarget = target;
    }

    public void ClearChaseTarget()
    {
        chaseTarget = null;
    }
}