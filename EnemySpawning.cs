using UnityEngine;

public class EnemySpawning : MonoBehaviour
{
    private float _secondsCounter;
    [SerializeField] private GameObject enemy;
    private int enemyAmount;

    void Update()
    {
        this._secondsCounter += Time.deltaTime;
        if (this._secondsCounter > 12)  
        {
            this.enemyAmount = Random.Range(2, 4);
            for (int i = 0; i < this.enemyAmount; i++)
            {
                Instantiate(this.enemy, new Vector2(0, 0), Quaternion.identity);
                Instantiate(this.enemy, new Vector2(-1, -8), Quaternion.identity);
                Instantiate(this.enemy, new Vector2(-19, -8), Quaternion.identity);
            }

            this._secondsCounter = 0;
        }
    }
}
