using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyAI : MonoBehaviour
    {
        // Serialized fields to allow modification from the Unity Editor (Variablen Definieren)
        [SerializeField] private float roamChangeDirectionFloat = 2f; 
        [SerializeField] private float attackCooldown = 2f; 
        [SerializeField] private MonoBehaviour enemyType; 
        [SerializeField] private float attackRange = 5f; 

        private bool canAttack = true;

        // Enum to define the states of the enemy (Enum zur Definition der Zustände des Objekt)
        private enum State
        {
            Roaming, 
            Attacking 
        }
        
        private Vector2 roamPosition; 
        private float timeRoaming = 0f; 
        private State _state; 
        private EnemyPathfinding _enemyPathfinding; 

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // Initialize the enemy pathfinding component and set the initial state to Roaming
            this._enemyPathfinding = GetComponent<EnemyPathfinding>();
            this._state = State.Roaming;
        }

        // Start is called before the first frame update
        private void Start()
        {
            this.roamPosition = GetRoamingPosition();
        }

        // Update is called once per frame
        private void Update()
        {
            MovementStateControl();
        }

        // Controls the movement state of the enemy based on its current state
        private void MovementStateControl()
        {
            switch (this._state)
            {
                default:
                case State.Roaming:
                    Roaming(); 
                    break;
                case State.Attacking: 
                    Attacking(); 
                    break;
            }
        }
        
        private void Roaming()
        {
            this.timeRoaming += Time.deltaTime; //Time.deltaTime heißt aktuelle sek./ tik

            this._enemyPathfinding.MoveTo(this.roamPosition);

            // Check if the player is within attack range (Prüfen, ob der Spieler in Angriffsreichweite ist)
            if (Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) < this.attackRange)
            {
                this._state = State.Attacking;
            }

            // Change roam direction after a certain time
            if (this.timeRoaming > this.roamChangeDirectionFloat)
            {
                this.roamPosition = GetRoamingPosition();
            }
        }
        
        private void Attacking()
        {
            // Check if the player is out of attack range (Prüft ob spieler nicht mehr in Angriffsreichweite ist)
            if (Vector2.Distance(this.transform.position, PlayerController.Instance.transform.position) > this.attackRange)
            {
                this._state = State.Roaming; 
                this.roamPosition = GetRoamingPosition();
            }
            else
            {
                // Move towards the player's position (Auf die Position des Spielers zugehen)
                Vector2 playerPosition = PlayerController.Instance.transform.position;
                this._enemyPathfinding.MoveTo(playerPosition);

                if (this.canAttack)
                {
                    this.canAttack = false; // Prevent further attacks until cooldown is over

                    StartCoroutine(AttackCooldownRoutine());
                }
            }
        }

        // IEnumerator is used for creating coroutines, which allow execution to be paused and resumed
        // (behandelt meist alles, was mit Zeit zu tun hat)
        private IEnumerator AttackCooldownRoutine()
        {
            // Wait for the attack cooldown duration
            yield return new WaitForSeconds(this.attackCooldown);
            this.canAttack = true;
        }

        // Generate a new random roaming position to move to
        private Vector2 GetRoamingPosition()
        {
            this.timeRoaming = 0f;
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}
