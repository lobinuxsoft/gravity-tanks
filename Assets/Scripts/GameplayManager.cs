using UnityEngine;
using HNW;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] GameOverUI gameOverUI;

    Damageable player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
        player.onDie.AddListener(ShowGameOver);

        gameOverUI.OnRevivePress += Revive;
    }

    private void OnDestroy()
    {
        player.onDie.RemoveListener(ShowGameOver);
        gameOverUI.OnRevivePress -= Revive;
    }

    private void ShowGameOver()
    {
        gameOverUI.Show();
    }

    private void Revive()
    {
        player.FullHeal();

        player.transform.position = MapGenerator.Instance.GetMapCentrePos();

        player.gameObject.SetActive(true);
    }
}