using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HNW
{
    public class DebugView : MonoBehaviour
    {
        [SerializeField] HolographicButton closeButton;

        [Space(10)]
        [Header("Logger Settings")]
        [SerializeField] HolographicButton savelogButton;
        [SerializeField] HolographicButton clearlogButton;
        [SerializeField] TextMeshProUGUI textLogger;
        [SerializeField] Scrollbar scrollbar;
        LoggerBase logger;

        UIPopup popup;

        private void Awake()
        {
            popup = GetComponent<UIPopup>();
            closeButton.onClick += Hide;

            #region Logger
            savelogButton.onClick += SaveLog;
            clearlogButton.onClick += ClearLog;

            logger = LoggerBase.CreateLogger();
            logger.textMesh = textLogger;
            textLogger.text = "";
            Application.logMessageReceived += OnLogMessageReceived;
            #endregion
        }

        private void Start() => StartCoroutine(ShowLogs());

        private void OnDestroy()
        {
            closeButton.onClick -= Hide;

            #region Logger
            savelogButton.onClick -= SaveLog;
            clearlogButton.onClick -= ClearLog;

            Application.logMessageReceived -= OnLogMessageReceived;

            if (logger != null)
                logger = null;
            #endregion
        }

        private void SaveLog() => logger.SaveLogs();

        private void ClearLog() => logger.ClearLogs();

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            Color color = Color.white;
            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                case LogType.Assert:
                    color = Color.red;
                    break;

                case LogType.Warning:
                    color = Color.yellow;
                    break;
                case LogType.Log:
                    color = Color.green;
                    break;
            }

            logger.Log(
                $"<b><color=#{ColorUtility.ToHtmlStringRGB(color)}>[{type.ToString()}]</color></b>: {condition}."
            );

            StartCoroutine(ShowLogs());
        }

        IEnumerator ShowLogs()
        {
            logger.ShowLogs();

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)textLogger.transform.parent);

            yield return new WaitForEndOfFrame();

            scrollbar.value = 0;
        }

        public void Show() => popup.Show(null, null, 2);

        private void Hide() => popup.Hide(null, null, 2);
    }
}