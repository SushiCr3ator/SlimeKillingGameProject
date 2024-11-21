using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        this._particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (this._particleSystem && !this._particleSystem.IsAlive())
        {
            DestroySelf();
        }
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
