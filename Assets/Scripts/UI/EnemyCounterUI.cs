using CryingOnionTools.ScriptableVariables;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace GravityTanks.UI
{
    public class EnemyCounterUI : MonoBehaviour
    {
        [SerializeField] IntVariable enemiesAmount;
        UIDocument uiDocument;
        Label enemyCounterLabel;

        int totalEnemies;

        public UnityEvent onAllEnemyDetroy;

        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            enemyCounterLabel = uiDocument.rootVisualElement.Q<Label>("enemy-counter-label");
            enemiesAmount.onValueChange += UpdateView;
            FindAllEnemies();
        }

        private void OnDestroy() => enemiesAmount.onValueChange -= UpdateView;

        private void FindAllEnemies()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            totalEnemies = enemies.Length;
            enemiesAmount.Value = totalEnemies;

            foreach (var enemy in enemies)
            {
                if (enemy.TryGetComponent(out Damageable damageable)) 
                    damageable.onDie.AddListener(() => enemiesAmount.Value--);
            }
        }

        private void UpdateView(int value)
        {
            enemyCounterLabel.text = $"x{value:00}";
            if (value <= 0) onAllEnemyDetroy?.Invoke();
        }
    }
}