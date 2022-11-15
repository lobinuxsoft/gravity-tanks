using UnityEngine;
using HNW;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class GameplayManager : MonoBehaviour
{
    [SerializeField] LongVariable killEnemiesAmount;
    [SerializeField] ShipData shipData;
    [SerializeField] GameObject farCamera;
    [SerializeField] GameObject nearCamera;
    [SerializeField] GameOverUI gameOverUI;
    [SerializeField] string coreLoopSceneName = "Core Loop";
    [SerializeField] Gradient fadeIn;
    [SerializeField] Gradient fadeOut;

    Damageable player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
        player.onDie.AddListener(ShowGameOver);

        gameOverUI.OnRevivePress += Revive;
        gameOverUI.OnReturnPress += Return;
    }

    private void Start()
    {
        shipData.BuildShip(player.transform);
    }

    private void OnDestroy()
    {
        player.onDie.RemoveListener(ShowGameOver);
        gameOverUI.OnRevivePress -= Revive;
        gameOverUI.OnReturnPress -= Return;
        killEnemiesAmount.EraseData();
    }

    private void ShowGameOver(GameObject go)
    {
        farCamera.SetActive(false);
        nearCamera.SetActive(true);
        gameOverUI.Show();

        #if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportScore(killEnemiesAmount.Value, GPGSIds.leaderboard_psico_killer, (bool success) => { });
        #endif

        killEnemiesAmount.EraseData();
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

    private void Return()
    {
        TimelineUITransition.Instance.FadeStart(coreLoopSceneName, 1, fadeIn, fadeOut);
    }
}