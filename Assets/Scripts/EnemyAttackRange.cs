using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private EnemyController _enemyController;
    private Transform _target;

    void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
    }

    void Update()
    {
        // E�er _target varsa, aktif olup olmad���n� kontrol et
        if (_target != null)
        {
            if (!_target.gameObject.activeInHierarchy)
            {
                // Oyuncu �ld�yse veya devre d��� kald�ysa hedefi temizle
                _enemyController.ClearChaseTarget();
                _target = null;
            }
            else
            {
                // Hedef aktifse enemy'nin hedefini g�ncelle
                _enemyController.SetChaseTarget(_target);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _target = collision.transform;
            _enemyController.SetChaseTarget(_target);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Hedefi s�rekli g�ncelle
            _target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _enemyController.ClearChaseTarget();
            _target = null;
        }
    }
}
