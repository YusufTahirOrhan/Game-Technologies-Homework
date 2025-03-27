using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public HitboxController SwordHitbox; // Inspector üzerinden atayýnýz

    // Bu metot animasyon event'i tarafýndan tetiklenir
    public void OnAttackEvent()
    {
        if (SwordHitbox != null)
        {
            SwordHitbox.ActivateHitbox();
        }
    }
}
