using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace MathLab3;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeChart()
    {
        Lab3Chart chart = new()
        {
            Parent = this
        };
        InitializeExplictMethodChart(chart);
        InitializeImplctMethodChart(chart);
        InitializeAnMethodChart(chart);

    }

    private void InitializeAnMethodChart(Lab3Chart chart)
    {
        var areaData = chart.GetChartArea("Analitic");
        AddStandartTSeries(chart, areaData);

        int time = 0;
        while (time < MathFormatter.MaxT)
        {
            if (time == 0) time += 10;
            else if (time == 10) time += 15;
            else time += 25;

            var series = chart.CreateSeries(areaData, $"{areaData.Area.Name} time {time}");

            for(int i = 0; i < MathFormatter.NumX; i++)
            {
                double y = MathFormatter.UFourier(i * MathFormatter.StepSpace, time, 100);
                series.Points.AddXY(i, y);
            }
        }
    }

    private void InitializeImplctMethodChart(Lab3Chart chart)
    {
        var areaData = chart.GetChartArea("Implict");
        AddStandartTSeries(chart, areaData);

        var result = MathFormatter.SolveImplicitMethod();
        int lineLength = result.GetUpperBound(1) + 1;
        int rLine = result.GetUpperBound(0);

        int time = 0;
        int timeIndex = 0;
        while (time < MathFormatter.MaxT)
        {
            if (time == 0) time += 10;
            else if (time == 10) time += 15;
            else time += 25;
            timeIndex = (int)(time / MathFormatter.StepTime);

            var series = chart.CreateSeries(areaData, $"{areaData.Area.Name} time {time}");
            for(int i = 0; i < lineLength; i++)
            {
                series.Points.AddXY(i, result[timeIndex, i]);
            }
        }
        
    }

    private void InitializeExplictMethodChart(Lab3Chart chart)
    {
        var areaData = chart.GetChartArea("Explict");
        AddStandartTSeries(chart, areaData);

        double time = 0;
        double pTime = 0;
        var temp = MathFormatter.StartFunc();
        while (time < MathFormatter.MaxT)
        {
            if (time == 0) time += 10;
            else if (time == 10) time += 15;
            else time += 25;

            temp = MathFormatter.ExplictMethod(time,pTime, temp);

            var series = chart.CreateSeries(areaData, $"{areaData.Area.Name} time {time}");
            for(int i = 0; i < temp.Length; i++)
            {
                series.Points.AddXY(i, temp[i]);
            }
        }
    }

    private void AddStandartTSeries(Lab3Chart chart, ChartAreaData areaData)
    {
        var standartSeries = chart.CreateSeries(areaData, $"{areaData.Area.Name} standart temperature");
        double[] standartT = MathFormatter.StartFunc();
        for (int i = 0; i < standartT.Length; i++)
        {
            standartSeries.Points.AddXY(i, standartT[i]);
        }
    }

    /*private void InitializeCompareChart(Lab3Chart chart)
    {
        int time1 = 10;
        int time2 = 100;
        var temp = MathFormatter.StartFunc();

        var areaData = chart.GetChartArea("Compare");

        var expResult1 = MathFormatter.ExplictMethod(time1, 0, temp);
        var expResult2 = MathFormatter.ExplictMethod(time2, 0, temp);

        var impResult = MathFormatter.SolveImplicitMethod();

        var anSeries1 = chart.CreateSeries(areaData, $"{areaData.Area.Name} analitic time {time1}");
        var anSeries2 = chart.CreateSeries(areaData, $"{areaData.Area.Name} analitic time {time2}");

        for (int i = 0; i < MathFormatter.NumX; i++)
        {
            double y = MathFormatter.UFourier(i * MathFormatter.StepSpace, time1, 100);
            anSeries1.Points.AddXY(i, y);
            y = MathFormatter.UFourier(i * MathFormatter.StepSpace, time2, 100);
            anSeries2.Points.AddXY(i, y);
        }

        int timeIndex1 = (int)(time1 / MathFormatter.StepTime);
        int timeIndex2 = (int)(time2 / MathFormatter.StepTime);
        var imSeries1 = chart.CreateSeries(areaData, $"{areaData.Area.Name} implict time {time1}");
        var imSeries2 = chart.CreateSeries(areaData, $"{areaData.Area.Name} implict time {time2}");
        for (int i = 0; i < impResult.GetUpperBound(1)+1; i++)
        {
            imSeries1.Points.AddXY(i, impResult[timeIndex1, i]);
            imSeries2.Points.AddXY(i, impResult[timeIndex2, i]);
        }


    }*/

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Form1";
    }

    #endregion
}
