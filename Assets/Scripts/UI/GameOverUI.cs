using HNW;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPopup))]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] HolographicButton reviveButton;
    [SerializeField] HolographicButton returnButton;

    public event Action OnRevivePress;
    public event Action OnReturnPress;

    private UIPopup popup;

    private void Awake()
    {
        popup = GetComponent<UIPopup>();
        reviveButton.onClick += Revive;
        returnButton.onClick += Return;
    }

    private void OnDestroy()
    {
        reviveButton.onClick -= Revive;
        returnButton.onClick -= Return;
    }

    public void Show()
    {
        popup.Show();
    }

    private void Revive()
    {
        popup.Hide(null, OnRevivePress);
    }

    private void Return()
    {
        popup.Hide(null, OnReturnPress);
    }
}