using UnityEngine;
using UnityEngine.UI;

namespace Mangement
{
    public class KillCounterManager : MonoBehaviour
    {
        public static KillCounterManager Instance;

        [SerializeField] private Text killCounterText;
        private int killCount;
    
        // Event to notify subscribers when a kill is added
        public event System.Action OnKillAdded;

        private void Awake()
        {
            // Singleton pattern to ensure there's only one instance of KillCounterManager
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject); // Optionally, persist this across scenes
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            UpdateKillCounterText();
        }
    
        public void AddKill()
        {
            this.killCount++;
            UpdateKillCounterText();
            // Notify subscribers that a kill has been added
            OnKillAdded?.Invoke();
        }

        private void UpdateKillCounterText()
        {
            this.killCounterText.text = $"Slimes killed: {this.killCount}";
        
        }

        // Return killcount to use in playerhealth
        public int GetKillCount()
        {
            return this.killCount;
        }
    
    }
}
