using System.IO;

public class DefaultLogger : LoggerBase
{
    private string filepath;

    private string logs;

    public DefaultLogger(string filepath)
    {
        this.filepath = filepath;

        if (File.Exists(filepath))
            logs = File.ReadAllText(filepath);
    }

    public override void ClearLogs()
    {
        logs = "";

        textMesh.text = "";

        if (File.Exists(filepath))
            File.Delete(filepath);
    }

    public override void ShowLogs() => textMesh.text = logs;

    public override void Log(string message) => logs += $"{message}\n";

    public override void SaveLogs() => File.WriteAllText(filepath, logs);
}