using UnityEngine;

public class PlayerComponentBase : MonoBehaviour
{
    public virtual void OnPlayerDeath()
    {
        this.enabled = false;
    }
}
