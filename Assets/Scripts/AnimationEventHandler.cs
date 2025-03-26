using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public HitboxController swordHitbox; // Inspector üzerinden atayýnýz

    // Bu metot animasyon event'i tarafýndan tetiklenir
    public void OnAttackEvent()
    {
        if (swordHitbox != null)
        {
            swordHitbox.ActivateHitbox();
        }
    }
}
