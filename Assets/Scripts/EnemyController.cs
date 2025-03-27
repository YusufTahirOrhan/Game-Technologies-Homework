using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour, IAttack, IDamageable
{
    [Header("Hareket Ayarlarý")]
    public float RoveSpeed = 2f;
    public float IdleWaitTime = 1.0f; // Noktada bekleme süresi

    [Header("Saðlýk ve Hasar Ayarlarý")]
    public int MaxHealth = 50;
    private int _currentHealth;

    [Header("Saldýrý Ayarlarý")]
    public float AttackCooldown = 1f; // Saldýrýlar arasý bekleme süresi
    private float _lastAttackTime = 0f;
    public float AttackRangeDistance = 1.5f; // Oyuncuya yeterince yaklaþýnca saldýrý yapýlacak mesafe

    [Header("Gezme Noktalarý")]
    public Transform[] MovePoints; // Düþmanýn dolaþacaðý noktalar
    private int _currentPointIndex = 0;
    private bool _isWaiting = false;

    public Transform Character; // Sprite'ýn yönünü deðiþtirmek için referans

    private Rigidbody2D _rigidBody;
    private Animator _animator;

    // Takip modu için: Eðer oyuncu attack range içinde ise buraya atanýr.
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

        // Eðer oyuncu takip modunda varsa
        if (_chaseTarget != null)
        {
            Vector2 direction = ((Vector2)_chaseTarget.position - _rigidBody.position).normalized;
            direction.y = 0f;
            _rigidBody.linearVelocity = direction * RoveSpeed;

            // Karakterin yönünü ayarla
            if (direction.x < -0.1f)
            {
                Character.localScale = new Vector3(-Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            else if (direction.x > 0.1f)
            {
                Character.localScale = new Vector3(Mathf.Abs(Character.lossyScale.x), Character.lossyScale.y, Character.lossyScale.z);
            }
            _animator.SetBool("IsRunning", true);

            // Eðer oyuncuya yeterince yakýnsa saldýr
            float dist = Vector2.Distance(_rigidBody.position, _chaseTarget.position);
            if (dist <= AttackRangeDistance)
            {
                Attack();
            }
        }
        else
        {
            // Takip modunda deðilse, patrol davranýþý
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

                    // Noktaya ulaþýnca bekle
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

    // Saldýrý metodunda cooldown mekanizmasý
    public void Attack()
    {
        if (Time.time < _lastAttackTime + AttackCooldown)
            return;

        _lastAttackTime = Time.time;
        _animator.SetTrigger("Attack");
        Debug.Log("Düþman saldýrýyor!");
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        _animator.SetTrigger("Hurt");
        Debug.Log("Düþman " + damageAmount + " hasar aldý!");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _animator.SetBool("IsDead", true);
        _animator.SetTrigger("Die");
        Debug.Log("Düþman öldü!");
        _isDead = true;
        _rigidBody.linearVelocity = Vector2.zero;
        _rigidBody.gravityScale = 0f;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    // Bu metodlar, AttackRange collider'ýna baðlý script tarafýndan çaðrýlacak
    public void SetChaseTarget(Transform target)
    {
        _chaseTarget = target;
    }

    public void ClearChaseTarget()
    {
        _chaseTarget = null;
    }
}