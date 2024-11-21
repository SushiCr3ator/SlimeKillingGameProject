using System.Collections;
using Mangement;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    private int killCount;
    
    private int _currentHealth;
    private Knockback _knockback;
    private Flash _flash;
    
    private void Awake()
    {
        this._flash = GetComponent<Flash>();
        this._knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        this._currentHealth = this.startingHealth;
    }

    public void TakeDamage(int damage)
    {
        this._currentHealth -= damage;
        this._knockback.GetKnockedBack(PlayerController.Instance.transform, this.knockBackThrust);
        StartCoroutine(this._flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    // Remove the "white mask" of the enemy if not dead after some seconds
    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(this._flash.GetRestoreMaterialTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (this._currentHealth <= 0)
        {
            // Show death animation, add kill to counter and destroy the object
            Instantiate(this.deathVFXPrefab, transform.position, Quaternion.identity);
            KillCounterManager.Instance.AddKill();
            Destroy(gameObject);
        }
    }
    
}
