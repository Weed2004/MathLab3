using System.Windows.Forms.DataVisualization.Charting;

namespace MathLab3;

internal class Lab3Chart : Chart
{
    public Lab3Chart()
    {
        this.Dock = DockStyle.Fill;
    }

    public ChartAreaData GetChartArea(string name)
    {
        var area = this.ChartAreas.FirstOrDefault(x => x.Name == name);
        if (area is null)
        {
            area = new ChartArea(name);
            this.ChartAreas.Add(area);
            var legend = new Legend();
            this.Legends.Add(legend);

            ChartAreaData data = new(area, legend);

            return data;
        }
        else
        {
            throw new Exception("Area already exist");
        }
    }

    public Series CreateSeries(ChartAreaData data, SeriesChartType type = SeriesChartType.Line)
    {
        Series series = new Series()
        {
            ChartArea = data.Area.Name,
            BorderWidth = 3,
            ChartType = type,
            Legend = data.Legend.Name
        };
        this.Series.Add(series);
        return series;
    }

    public Series CreateSeries(ChartAreaData data, string name, SeriesChartType type = SeriesChartType.Line)
    {
        var series = CreateSeries(data, type);
        series.Name = name;
        return series;
    }
}
