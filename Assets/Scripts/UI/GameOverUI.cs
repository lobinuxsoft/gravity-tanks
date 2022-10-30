using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIPopup))]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] Button reviveButton;

    public event Action OnRevivePress;

    private UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();
        reviveButton.onClick.AddListener(Revive);
    }

    private void OnDestroy()
    {
        reviveButton.onClick.RemoveListener(Revive);
    }

    public void Show()
    {
        popup.Show();
    }

    private void Revive()
    {
        popup.Hide(null, OnRevivePress);
    }
}