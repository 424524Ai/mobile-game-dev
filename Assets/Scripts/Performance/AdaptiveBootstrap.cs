using UnityEngine;
using UnityEngine.AdaptivePerformance;

// Monitors Android thermal status and performance bottleneck events.
public class AdaptiveBootstrap : MonoBehaviour
{
    private IAdaptivePerformance ap;

    private void Start()
    {
        ap = Holder.Instance;

        if (ap == null || !ap.Active)
        {
            Debug.Log("[AP] provider inactive on this device");
            return;
        }

        ap.ThermalStatus.ThermalEvent += OnThermal;
        ap.PerformanceStatus.PerformanceBottleneckChangeEvent += OnBottleneck;

        var m = ap.ThermalStatus.ThermalMetrics;

        Debug.Log($"[AP] active warning={m.WarningLevel} level={m.TemperatureLevel:0.00}");
    }

    private void OnDestroy()
    {
        if (ap == null) return;

        ap.ThermalStatus.ThermalEvent -= OnThermal;
        ap.PerformanceStatus.PerformanceBottleneckChangeEvent -= OnBottleneck;
    }

    private void OnThermal(ThermalMetrics m)
    {
        Debug.Log($"[AP] thermal warning={m.WarningLevel} " +
                  $"level={m.TemperatureLevel:0.00} trend={m.TemperatureTrend:0.00}");
    }

    private void OnBottleneck(PerformanceBottleneckChangeEventArgs e)
    {
        Debug.Log($"[AP] bottleneck={e.PerformanceBottleneck}");
    }
}