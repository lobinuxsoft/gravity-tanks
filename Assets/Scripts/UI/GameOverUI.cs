using CryingOnionTools.ScriptableVariables;
using HNW;
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(UIPopup))]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] HolographicButton reviveButton;
    [SerializeField] HolographicButton returnButton;
    [SerializeField] TextMeshProUGUI reviveLabel;

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

    public void Show(bool showReviveButton, int reviveCost)
    {
        reviveButton.gameObject.SetActive(showReviveButton);
        reviveLabel.text = $"Revive -{reviveCost}Exp";
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