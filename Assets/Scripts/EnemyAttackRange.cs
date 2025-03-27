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
        // Eðer _target varsa, aktif olup olmadýðýný kontrol et
        if (_target != null)
        {
            if (!_target.gameObject.activeInHierarchy)
            {
                // Oyuncu öldüyse veya devre dýþý kaldýysa hedefi temizle
                _enemyController.ClearChaseTarget();
                _target = null;
            }
            else
            {
                // Hedef aktifse enemy'nin hedefini güncelle
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
            // Hedefi sürekli güncelle
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
