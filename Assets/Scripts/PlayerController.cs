using UnityEngine;

public class PlayerController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Can Ayarları")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Saldırı Ayarları")]
    public float attackCooldown = 1.3f;  // Saldırılar arası bekleme süresi
    private float lastAttackTime = 0f;

    public Transform Character;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded = true;

    // Input değerlerini saklamak için değişkenler
    private float horizontalInput = 0f;
    private bool jumpRequest = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Girişleri al
        horizontalInput = Input.GetAxis("Horizontal");

        // Karakterin dönmesi: hareket yönüne göre scale'ı güncelle
        if (horizontalInput < -0.1f)
        {
            // Sol tarafa bak
            Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
        }
        else if (horizontalInput > 0.1f)
        {
            // Sağ tarafa bak
            Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
        }
        anim.SetBool("IsRunning", Mathf.Abs(horizontalInput) > 0.1f);

        // Zıplama kontrolü
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequest = true;
            anim.SetBool("IsGrounded", false);
            anim.SetTrigger("Jump");
        }

        // Saldırı kontrolü
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        // Yatay hareket uygulama
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Zıplama işlemi
        if (jumpRequest)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            jumpRequest = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Yere temas kontrolü (Collider tag'ı "Ground" olmalı)
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("IsGrounded", true);
        }
    }

    public void Attack()
    {
        // Cooldown kontrolü: Eğer son saldırı cooldown süresi dolmamışsa, saldırıyı iptal et.
        if (Time.time < lastAttackTime + attackCooldown)
        {
            return;
        }

        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        Debug.Log("Player saldırıyor!");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("Hurt");
        Debug.Log("Player " + damage + " hasar aldı!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Die");
        Debug.Log("Player öldü!");
        this.enabled = false;
    }
}
