using System.Windows.Forms.DataVisualization.Charting;

namespace MathLab3;

internal record ChartAreaData
{
    public ChartArea Area { get; init; }
    public Legend Legend { get; init; }
    public ChartAreaData(ChartArea area, Legend legend)
    {
        Area = area;

        area.AxisX.Interval = 20;
        area.AxisX.Minimum = 0;

        legend.DockedToChartArea = area.Name;
        Legend = legend;
    }
}
