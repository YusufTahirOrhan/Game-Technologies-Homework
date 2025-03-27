using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public HitboxController SwordHitbox; // Inspector �zerinden atay�n�z

    // Bu metot animasyon event'i taraf�ndan tetiklenir
    public void OnAttackEvent()
    {
        if (SwordHitbox != null)
        {
            SwordHitbox.ActivateHitbox();
        }
    }
}
