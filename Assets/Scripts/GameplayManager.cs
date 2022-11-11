using UnityEngine;
using HNW;
using CryingOnionTools.ScriptableVariables;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class GameplayManager : MonoBehaviour
{
    [SerializeField] LongVariable killEnemiesAmount;
    [SerializeField] WeaponData startWeapon;
    [SerializeField] ChassisData startChassis;
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

    private void Start()
    {
        startChassis.BuildChassis(player.transform);

        if (player.TryGetComponent(out ShootControl sc))
        {
            startWeapon.BuildWeapon(sc.transform);
            sc.UpdateWeapons();
        }

        #if UNITY_ANDROID
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        #endif
    }

    private void OnDestroy()
    {
        player.onDie.RemoveListener(ShowGameOver);
        gameOverUI.OnRevivePress -= Revive;
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

    public void ShowAchievements()
    {
        #if UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowAchievementsUI();
        #endif
    }


#if UNITY_ANDROID
    void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log($"Authentication statuc: {status.ToString()}");

        switch (status)
        {
            case SignInStatus.Success:
                PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_hello_world, 100.0f, (bool success) => { });
                break;
        }
    }
#endif
}