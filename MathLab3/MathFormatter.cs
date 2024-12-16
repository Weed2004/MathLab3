namespace MathLab3;

internal class MathFormatter
{
    public static double StepSpace { get; set; } = 1.0;
    public static double StepTime { get; set; } = 0.05;
    public static double TermalC { get; set; } = 1.0;
    public static int MaxT { get; set; } = 100;
    public static double StabilityParam { get; private set; } = TermalC * StepTime / (StepSpace * StepSpace);
    public static int NumX { get; set; } = 100;

    public static double[] StartFunc()
    {
        double[] y = new double[NumX];

        for (int i = 0; i < NumX; i++)
        {
            double x = i * StepSpace;
            if (x >= 50 && x <= 60) y[i] = 0.1 * x - 5;
            else if (x > 60 && x <= 70) y[i] = -0.01 * Math.Pow(x - 60, 2) + 1;
            else y[i] = 0;
        }
        return y;
    }

    public static double[] ExplictMethod(double maxT, double startT, double[] t)
    {
        double[] tResult = new double[t.Length];

        for (double i = startT; i < maxT; i += StepTime)
        {
            for (int j = 1; j < t.Length - 1; j++)
            {
                tResult[j] = t[j] + StabilityParam * (t[j - 1] - 2 * t[j] + t[j + 1]);
            }
            t = tResult;
        }

        return tResult;
    }

    public static double[,] SolveImplicitMethod()
    {
        int timeSteps = (int)(MaxT / StepTime) + 1;
        double[,] u = new double[timeSteps, NumX + 1];

        for (int i = 0; i <= NumX; i++)
        {
            u[0, i] = InitialDistribution(i * StepSpace);
        }

        double r = TermalC * StepTime / (StepSpace * StepSpace);

        double[] A = new double[NumX - 2];
        double[] B = new double[NumX - 1];
        double[] C = new double[NumX - 2];

        for (int i = 0; i < NumX - 2; i++)
        {
            A[i] = -r;
            C[i] = -r;
        }

        for (int i = 0; i < NumX - 1; i++)
        {
            B[i] = 1 + 2 * r;
        }

        double[] P = new double[NumX - 1];
        double[] Q = new double[NumX - 1];
        double[] precomputedInvDenominators = new double[NumX - 2];

        double invDenominator = 1 / B[0];
        P[0] = C[0] * invDenominator;
        precomputedInvDenominators[0] = invDenominator;

        for (int i = 1; i < NumX - 2; i++)
        {
            precomputedInvDenominators[i] = 1 / (B[i] - A[i - 1] * P[i - 1]);
            P[i] = C[i] * precomputedInvDenominators[i];
        }

        for (int n = 0; n < (int)(MaxT / StepTime); n++)
        {
            double[] d = new double[NumX - 1];
            for (int i = 0; i < NumX - 1; i++)
            {
                d[i] = u[n, i + 1];
            }

            Q[0] = d[0] * precomputedInvDenominators[0];
            for (int i = 1; i < NumX - 2; i++)
            {
                Q[i] = (d[i] - A[i - 1] * Q[i - 1]) * precomputedInvDenominators[i];
            }

            u[n + 1, NumX - 1] = Q[NumX - 2];
            for (int i = NumX - 3; i >= 0; i--)
            {
                u[n + 1, i + 1] = Q[i] - P[i] * u[n + 1, i + 2];
            }

            u[n + 1, 0] = u[n + 1, 1];
            u[n + 1, NumX] = u[n + 1, NumX - 1];
        }

        return u;
    }

    static double IntCalculateBn(int n)
    {
        if (n <= 0)
            throw new ArgumentException("n должно быть больше 0.");

        double pi = Math.PI;

        // Первый интеграл
        double firstIntegral = ((double)1000 * Math.Sin(pi * n * 3 / 5) - (double)100 * pi * n * Math.Cos(pi * 3 * n / 5)
            - (double)1000 * Math.Sin(pi * n / 2)) / (pi * pi * n * n);

        // Второй интеграл
        //double secondIntegral = (double)((double)2000 * pi * n * Math.Sin((double)7 * pi * n / 10) + ((double)20000 - (double)200 * pi * pi * n * n)
        //    * Math.Cos((double)7 * pi * n / 10) + ((double)100 * pi * pi * n * n - 20000)
        //    * Math.Cos((double)3 * pi * n / 5)) / (pi * pi * pi * n * n * n);
        double secondIntegral = ((double)2000 * pi * n * Math.Sin(pi * n * 7 / 10) + 20000 * Math.Cos(pi * n * 7 / 10)
            + ((double)-100 * pi * pi * n * n - 20000) * Math.Cos((double)3 * pi * n / 5)) / -
            (pi * pi * pi * n * n * n);

        double result = ((double)2 / NumX) * ((firstIntegral) + secondIntegral);

        return result;
    }

    static double[] ComputeFourierCoefficients(int nMax)
    {
        double[] coefficients = new double[nMax];
        for (int n = 1; n <= nMax; n++)
        {
            coefficients[n - 1] = IntCalculateBn(n);
        }
        return coefficients;
    }

    public static double UFourier(double x, double t, int nMax)
    {
        double[] bCoefficients = ComputeFourierCoefficients(nMax);

        double sum = 0;
        for (int n = 1; n <= nMax; n++)
        {
            double bn = bCoefficients[n - 1];
            //sum += bn * Math.Exp(-((double)n * Math.PI / NumX) * ((double)n * Math.PI / NumX) * TermalC * t)
            //    * Math.Sin((double)n * Math.PI * x / NumX);
            sum += bn * Math.Exp(-TermalC * Math.Pow((double)n * Math.PI / NumX, 2) * t) * Math.Sin((double)n * Math.PI * x / NumX);
        }
        return sum;
    }

    private static double InitialDistribution(double x)
    {
        if (x >= 50 && x <= 60) return 0.1 * x - 5;
        else if (x > 60 && x <= 70) return -0.01 * Math.Pow(x - 60, 2) + 1;
        else return 0;
    }
}
