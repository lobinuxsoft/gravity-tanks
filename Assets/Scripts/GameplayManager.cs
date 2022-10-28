using UnityEngine;
using HNW;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] GameObject farCamera;
    [SerializeField] GameObject nearCamera;
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
        farCamera.SetActive(false);
        nearCamera.SetActive(true);
        gameOverUI.Show();
    }

    private void Revive()
    {
        if(player.TryGetComponent(out Rigidbody body))
        {
            body.isKinematic = true;
            body.velocity -= body.velocity;
            body.isKinematic = false;
        }

        player.FullHeal();

        player.transform.position = MapGenerator.Instance.GetRandomPos() + Vector3.up * .5f;
        player.transform.rotation = Quaternion.identity;

        player.gameObject.SetActive(true);

        nearCamera.SetActive(false);
        farCamera.SetActive(true);
    }
}