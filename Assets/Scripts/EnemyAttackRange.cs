using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    private EnemyController enemyController;

    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Sadece oyuncu ile etkileþim: Tag "Player" kontrol ediliyor
        if (collision.CompareTag("Player"))
        {
            enemyController.SetChaseTarget(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemyController.ClearChaseTarget();
        }
    }
}
