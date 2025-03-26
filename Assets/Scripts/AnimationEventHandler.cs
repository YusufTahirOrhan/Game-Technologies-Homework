using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public HitboxController swordHitbox; // Inspector �zerinden atay�n�z

    // Bu metot animasyon event'i taraf�ndan tetiklenir
    public void OnAttackEvent()
    {
        if (swordHitbox != null)
        {
            swordHitbox.ActivateHitbox();
        }
    }
}
