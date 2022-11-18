using UnityEngine;
using CryingOnionTools.ScriptableVariables;
using System.Collections;

#if UNITY_ANDROID
using GooglePlayGames;
#endif

namespace HNW
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] LongVariable killEnemiesAmount;
        [SerializeField] ShipData shipData;
        [SerializeField] IntVariable curHp;
        [SerializeField] IntVariable maxHp;
        [SerializeField] LongVariable exp;
        [SerializeField] GameObject farCamera;
        [SerializeField] GameObject nearCamera;
        [SerializeField] GameOverUI gameOverUI;
        [SerializeField] NextWaveUI nextWaveUI;
        [SerializeField] string coreLoopSceneName = "Core Loop";
        [SerializeField] Gradient fadeIn;
        [SerializeField] Gradient fadeOut;

        Damageable player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Damageable>();
            player.onDie.AddListener(ShowGameOver);

            gameOverUI.OnRevivePress += OnReviveClicked;
            gameOverUI.OnReturnPress += OnReturnClicked;

            nextWaveUI.onHealClicked += OnHealClicked;
            nextWaveUI.onNextClicked += OnNextClicked;
            nextWaveUI.onReturnClicked += OnReturnClicked;

            Spawner.onWaveEnd += OnWaveEnd;
            Spawner.onAllWavesEnd += OnAllWavesEnd;
        }

        private void OnDestroy()
        {
            player.onDie.RemoveListener(ShowGameOver);

            gameOverUI.OnRevivePress -= OnReviveClicked;
            gameOverUI.OnReturnPress -= OnReturnClicked;

            nextWaveUI.onHealClicked -= OnHealClicked;
            nextWaveUI.onNextClicked -= OnNextClicked;
            nextWaveUI.onReturnClicked -= OnReturnClicked;

            Spawner.onWaveEnd -= OnWaveEnd;
            Spawner.onAllWavesEnd -= OnAllWavesEnd;

            killEnemiesAmount.EraseData();
        }

        private void Start()
        {
            shipData.BuildShip(player.transform);
        }

        private void ShowGameOver(GameObject go)
        {
            farCamera.SetActive(false);
            nearCamera.SetActive(true);
            gameOverUI.Show(exp.Value >= maxHp.Value * 10, maxHp.Value * 10);

#if UNITY_ANDROID
            PlayGamesPlatform.Instance.ReportScore(killEnemiesAmount.Value, GPGSIds.leaderboard_psico_killer, (bool success) => { });
#endif

            killEnemiesAmount.EraseData();
        }

        private void OnReviveClicked()
        {
            StartCoroutine(ResetPlayerPosition());

            exp.Value -= maxHp.Value * 10;
            player.FullHeal();

            nearCamera.SetActive(false);
            farCamera.SetActive(true);
        }

        private void OnReturnClicked()
        {
            Time.timeScale = 1;
            TimelineUITransitionScene.Instance.FadeStart(coreLoopSceneName, 1, fadeIn, fadeOut);
        }

        private void OnNextClicked()
        {
            Time.timeScale = 1;
            Spawner.Instance.NextWave();

            StartCoroutine(ResetPlayerPosition());
        }

        private void OnHealClicked()
        {
            exp.Value -= (maxHp.Value - curHp.Value) * 10;
            curHp.Value = maxHp.Value;
        }

        private void OnWaveEnd()
        {
            nextWaveUI.Show((curHp.Value < maxHp.Value && exp.Value >= (maxHp.Value - curHp.Value) * 10), (maxHp.Value - curHp.Value) * 10);
            Time.timeScale = 0;
        }

        private void OnAllWavesEnd()
        {
            // TODO pantalla final.
            Debug.Log("END GAME!!!");
        }

        IEnumerator ResetPlayerPosition()
        {
            yield return new WaitForEndOfFrame();
            player.gameObject.SetActive(false);

            if (player.TryGetComponent(out Rigidbody body))
            {
                body.isKinematic = true;
                body.velocity -= body.velocity;
                body.isKinematic = false;
            }

            player.transform.position = MapGenerator.Instance.GetRandomPos() + Vector3.up * .5f;
            player.transform.rotation = Quaternion.identity;

            player.gameObject.SetActive(true);
        }
    }
}