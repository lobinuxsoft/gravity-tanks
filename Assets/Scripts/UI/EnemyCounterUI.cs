using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GravityTanks.UI
{
    public class EnemyCounterUI : MonoBehaviour
    {
        UIDocument uiDocument;
        Label enemyCounterLabel;

        int totalEnemies;
        int enemiesAmount;

        public int TotalEnemiesDestroyed => totalEnemies - enemiesAmount;

        public UnityEvent onAllEnemyDetroy;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            enemyCounterLabel = uiDocument.rootVisualElement.Q<Label>("enemy-counter-label");
            FindAllEnemies();
        }

        private void FindAllEnemies()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            totalEnemies = enemies.Length;
            enemiesAmount = totalEnemies;

            enemyCounterLabel.text = $"x{enemiesAmount:00}";

            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out Damageable damageable)) 
                    damageable.onDie.AddListener(SubstractEnemy);
            }
        }

        private void SubstractEnemy()
        {
            enemiesAmount--;
            enemyCounterLabel.text = $"x{enemiesAmount:00}";

            if (enemiesAmount <= 0) onAllEnemyDetroy?.Invoke();
        }
    }
}