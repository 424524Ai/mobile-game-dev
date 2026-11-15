using System;
using System.IO;
using System.Text;
using UnityEngine;

// Records gameplay telemetry to the Unity console and a persistent local file.
public static class Telemetry
{
    private static readonly string session =
        Guid.NewGuid().ToString("N").Substring(0, 8);

    private static readonly string path =
        Path.Combine(Application.persistentDataPath, "telemetry.log");

    public static void Log(string ev, params (string k, object v)[] parameters)
    {
        var sb = new StringBuilder(128);

        sb.Append(Time.realtimeSinceStartup.ToString("F2"))
          .Append(' ')
          .Append(session)
          .Append(' ')
          .Append(ev);

        foreach (var (k, v) in parameters)
        {
            sb.Append(' ')
              .Append(k)
              .Append('=')
              .Append(v);
        }

        string line = sb.ToString();

        Debug.Log("[telemetry] " + line);
        File.AppendAllText(path, line + "\n");
    }
}