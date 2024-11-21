using System.Collections;
using Enemies;
using Mangement;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsDead { get; private set; }
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float knockbackThrustAmount = 10f;
        [SerializeField] private float damageRecoveryTime = 1f;
        [SerializeField] private GameObject deathCanvas;  // Reference to the DeathCanvas

        private int _currentHealth;
        private bool _canTakeDamage = true;
        private Knockback _knockback;
        private Flash _flash;

        private Slider healthSlider;

        private const string HealthSliderText = "HealthSlider";

        private void Awake()
        {
            this._flash = GetComponent<Flash>();
            this._knockback = GetComponent<Knockback>();
        }

        private void Start()
        {
            this.IsDead = false;
            this._currentHealth = this.maxHealth;
            // Subscribe to the kill notification event
            KillCounterManager.Instance.OnKillAdded += OnKillAdded;
                
            UpdateHealthSlider();
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            EnemyAI enemyAI = other.gameObject.GetComponent<EnemyAI>();

            if (enemyAI)
            {
                TakeDamage(1, other.transform);
            }
        }

        private void HealPlayer()
        {
            if (this._currentHealth < this.maxHealth)
            {
                this._currentHealth += 1;
                UpdateHealthSlider();
            }
        }

        private void TakeDamage(int damageAmount, Transform hitTransform)
        {
            if (!this._canTakeDamage) { return; }
        
            this._knockback.GetKnockedBack(hitTransform, this.knockbackThrustAmount);
            StartCoroutine(this._flash.FlashRoutine());
            this._canTakeDamage = false;
            this._currentHealth -= damageAmount;
            StartCoroutine(DamageRecoveryRoutine());
            UpdateHealthSlider();
            CheckIfPlayerDeath();
        }

        private void CheckIfPlayerDeath()
        {
            if (this._currentHealth <= 0)
            {
                this.IsDead = true;
                Time.timeScale = 0f;  // Freeze the game
                this.deathCanvas.SetActive(true);  // Show the DeathCanvas
            }
        }

        private void UpdateHealthSlider()
        {
            if (this.healthSlider == null)
            {
                this.healthSlider = GameObject.Find(HealthSliderText).GetComponent<Slider>();
            }

            this.healthSlider.maxValue = this.maxHealth;
            this.healthSlider.value = this._currentHealth;
        }

        private IEnumerator DamageRecoveryRoutine()
        {
            yield return new WaitForSeconds(this.damageRecoveryTime);
            this._canTakeDamage = true;
        }
    
        // Method to handle the kill event
        // ReSharper disable Unity.PerformanceAnalysis
        private void OnKillAdded()
        {
            // Check if 5 kills have been made
            if (this._currentHealth < 5)
            {
                if (KillCounterManager.Instance.GetKillCount() % 5 == 0)
                {
                    HealPlayer();
                }
            }
        }
    
    }
}
