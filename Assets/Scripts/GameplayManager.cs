using UnityEngine;
using HNW;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] WeaponData startWeapon;
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
        if (player.TryGetComponent(out ShootControl sc))
        {
            startWeapon.BuildWeapon(sc.transform);
            sc.UpdateWeapons();
        }

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    private void OnDestroy()
    {
        player.onDie.RemoveListener(ShowGameOver);
        gameOverUI.OnRevivePress -= Revive;
    }

    private void ShowGameOver(GameObject go)
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

    void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log($"Authentication statuc: {status.ToString()}");

        switch (status)
        {
            case SignInStatus.Success:
                Social.ReportProgress(GPGSIds.achievement_hello_world, 100f, (bool success) => {
                    if (success)
                        Debug.Log($"Hello World Achievement Success");
                    else
                        Debug.LogError($"Hello World Achievement Fail");
                });
                break;
        }
    }
}