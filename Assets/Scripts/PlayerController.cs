using UnityEngine;

public class PlayerController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket Ayarları")]
    public float MoveSpeed = 5f;
    public float JumpForce = 10f;

    [Header("Can Ayarları")]
    public int MaxHealth = 100;
    private int _currentHealth;

    [Header("Saldırı Ayarları")]
    public float AttackCooldown = 1.3f;  // Saldırılar arası bekleme süresi
    private float _lastAttackTime = 0f;

    public Transform Character;

    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private bool _isGrounded = true;

    // Input değerlerini saklamak için değişkenler
    private float _horizontalInput = 0f;
    private bool _jumpRequest = false;

    private bool _isDead = false;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = MaxHealth;
    }

    void Update()
    {
        if (_isDead && _isGrounded)
        {
            GetComponent<Collider2D>().enabled = false;
            _rigidBody.gravityScale = 0;
            _rigidBody.linearVelocity = Vector2.zero;
            return;
        }
        else if (_isDead)
        {
            return;
        }
        
        // Girişleri al
        _horizontalInput = Input.GetAxis("Horizontal");

        // Karakterin dönmesi: hareket yönüne göre scale'ı güncelle
        if (_horizontalInput < -0.1f)
        {
            // Sol tarafa bak
            Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
        }
        else if (_horizontalInput > 0.1f)
        {
            // Sağ tarafa bak
            Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
        }
        _animator.SetBool("IsRunning", Mathf.Abs(_horizontalInput) > 0.1f);

        // Zıplama kontrolü
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _jumpRequest = true;
            _animator.SetBool("IsGrounded", false);
            _animator.SetTrigger("Jump");
        }

        // Saldırı kontrolü
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (_isDead)
            return;

        // Yatay hareket uygulama
        _rigidBody.linearVelocity = new Vector2(_horizontalInput * MoveSpeed, _rigidBody.linearVelocity.y);

        // Zıplama işlemi
        if (_jumpRequest)
        {
            _rigidBody.linearVelocity = new Vector2(_rigidBody.linearVelocity.x, JumpForce);
            _isGrounded = false;
            _jumpRequest = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Yere temas kontrolü (Collider tag'ı "Ground" olmalı)
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            _animator.SetBool("IsGrounded", true);
        }
    }

    public void Attack()
    {
        // Cooldown kontrolü: Eğer son saldırı cooldown süresi dolmamışsa, saldırıyı iptal et.
        if (Time.time < _lastAttackTime + AttackCooldown)
        {
            return;
        }

        _lastAttackTime = Time.time;
        _animator.SetTrigger("Attack");
        Debug.Log("Player saldırıyor!");
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _animator.SetTrigger("Hurt");
        Debug.Log("Player " + damage + " hasar aldı!");

        InGameUI.Instance.UpdateHealthBar((float) _currentHealth / (float) MaxHealth * 100f);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _animator.SetBool("IsDead", true);
        _animator.SetTrigger("Die");
        Debug.Log("Player öldü!");
        _isDead = true;
        
        this.enabled = false;
    }
}
