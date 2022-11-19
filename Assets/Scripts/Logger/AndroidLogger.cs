using System;
using UnityEngine;

public class AndroidLogger : LoggerBase
{
    const string pluginName = "com.cryingonion.logger.Logger";
    const string interfaceName = "com.cryingonion.logger.AlertCallback";

    public class AlertCallback : AndroidJavaProxy
    {
        public Action positiveAction;
        public Action negativeAction;

        public AlertCallback() : base(interfaceName) { }

        public void onPositive() => positiveAction?.Invoke();

        public void onNegative() => negativeAction?.Invoke();
    }

    AndroidJavaClass loggerClass;
    AndroidJavaObject loggerObject;

    string filepath;

    public AndroidLogger(string filepath)
    {
        this.filepath = filepath;

        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");

        loggerClass = new AndroidJavaClass(pluginName);
        loggerObject = loggerClass.CallStatic<AndroidJavaObject>("getInstance", filepath);

        loggerObject.CallStatic("receiveUnityActivity", activity);

        Debug.Log($"Activity: {activity}");
    }

    public override void ClearLogs()
    {
        ShowAlert("Clear Logs and delete Logs.txt", "Are you sure?", () =>
        {
            loggerObject.Call("clearLogs");
            textMesh.text = "";
        });
    }

    public override void ShowLogs() => textMesh.text = loggerObject.Call<string>("getLogs");

    public override void Log(string message) => loggerObject.Call("sendLog", $"{message}\n");

    public override void SaveLogs()
    {
        ShowAlert("Save logs", $"The logs will be saved in {filepath}", () => loggerObject.Call("saveLog"));
    }

    private void ShowAlert(string title, string message, Action positive = null, Action negative = null)
    {
        AlertCallback alertCallback = new AlertCallback();
        alertCallback.positiveAction = positive;
        alertCallback.negativeAction = negative;

        loggerObject.Call("createAlert", new object[] { title, message, alertCallback });
        loggerObject.Call("showAlert");
    }
}
