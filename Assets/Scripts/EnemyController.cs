using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket Ayarlar�")]
    public float RoveSpeed = 2f;
    public float IdleWaitTime = 1.0f; // Noktada bekleme s�resi

    [Header("Sa�l�k ve Hasar Ayarlar�")]
    public int MaxHealth = 50;
    private int _currentHealth;

    [Header("Sald�r� Ayarlar�")]
    public float AttackCooldown = 1f; // Sald�r�lar aras� bekleme s�resi
    private float _lastAttackTime = 0f;
    public float AttackRangeDistance = 1.5f; // Oyuncuya yeterince yakla��nca sald�r� yap�lacak mesafe

    [Header("Gezme Noktalar�")]
    public Transform[] MovePoints; // D��man�n dola�aca�� noktalar
    private int _currentPointIndex = 0;
    private bool _isWaiting = false;

    public Transform Character; // Sprite'�n y�n�n� de�i�tirmek i�in referans

    private Rigidbody2D _rigidBody;
    private Animator _animator;

    // Takip modu i�in: E�er oyuncu attack range i�inde ise buraya atan�r.
    private Transform _chaseTarget;

    private bool _isDead = false;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = MaxHealth;
        _lastAttackTime = Time.time;
    }

    void FixedUpdate()
    {
        if(_isDead) return;

        // E�er oyuncu takip modunda varsa
        if (_chaseTarget != null)
        {
            Vector2 direction = ((Vector2)_chaseTarget.position - _rigidBody.position).normalized;
            direction.y = 0f;
            _rigidBody.linearVelocity = direction * RoveSpeed;

            // Karakterin y�n�n� ayarla
            if (direction.x < -0.1f)
            {
                Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            else if (direction.x > 0.1f)
            {
                Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            _animator.SetBool("IsRunning", true);

            // E�er oyuncuya yeterince yak�nsa sald�r
            float dist = Vector2.Distance(_rigidBody.position, _chaseTarget.position);
            if (dist <= AttackRangeDistance)
            {
                Attack();
            }
        }
        else
        {
            // Takip modunda de�ilse, patrol davran���
            if (!_isWaiting)
            {
                if (MovePoints.Length > 0)
                {
                    Transform target = MovePoints[_currentPointIndex];
                    Vector2 direction = ((Vector2)target.position - _rigidBody.position).normalized;
                    _rigidBody.linearVelocity = direction * RoveSpeed;

                    if (direction.x < -0.1f)
                    {
                        Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
                    }
                    else if (direction.x > 0.1f)
                    {
                        Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
                    }

                    // Noktaya ula��nca bekle
                    if (Vector2.Distance(_rigidBody.position, target.position) < 0.1f)
                    {
                        StartCoroutine(WaitAtPoint());
                    }
                    _animator.SetBool("IsRunning", true);
                }
                else
                {
                    _animator.SetBool("IsRunning", false);
                    _rigidBody.linearVelocity = new Vector2(0, _rigidBody.linearVelocity.y);
                }
            }
            else
            {
                _rigidBody.linearVelocity = Vector2.zero;
                _animator.SetBool("IsRunning", false);
            }
        }
    }

    IEnumerator WaitAtPoint()
    {
        _isWaiting = true;
        _rigidBody.linearVelocity = Vector2.zero;
        _animator.SetBool("IsRunning", false);
        yield return new WaitForSeconds(IdleWaitTime);
        _currentPointIndex = (_currentPointIndex + 1) % MovePoints.Length;
        _isWaiting = false;
    }

    // Sald�r� metodunda cooldown mekanizmas�
    public void Attack()
    {
        if (Time.time < _lastAttackTime + AttackCooldown)
            return;

        _lastAttackTime = Time.time;
        _animator.SetTrigger("Attack");
        Debug.Log("D��man sald�r�yor!");
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        _animator.SetTrigger("Hurt");
        Debug.Log("D��man " + damageAmount + " hasar ald�!");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _animator.SetBool("IsDead", true);
        _animator.SetTrigger("Die");
        Debug.Log("D��man �ld�!");
        _isDead = true;
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.gravityScale = 0f;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // Bu metodlar, AttackRange collider'�na ba�l� script taraf�ndan �a�r�lacak
    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
    }

    public void ClearChaseTarget()
    {
        _chaseTarget = null;
    }
}