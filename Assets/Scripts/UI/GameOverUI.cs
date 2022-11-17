using HNW;
using System;
using UnityEngine;

[RequireComponent(typeof(UIPopup))]
public class GameOverUI : MonoBehaviour
{
    const uint reviveCost = 100;

    [SerializeField] LongVariable exp;
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
        reviveButton.gameObject.SetActive(exp.Value >= reviveCost);
        popup.Show();
    }

    private void Revive()
    {
        exp.Value -= reviveCost;
        popup.Hide(null, OnRevivePress);
    }

    private void Return()
    {
        popup.Hide(null, OnReturnPress);
    }
}