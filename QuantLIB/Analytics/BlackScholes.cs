using JHLib.QuantLIB.Model;
using MathNet.Numerics.Distributions;
using System;

namespace JHLib.QuantLIB
{
    public enum OptionType { Call, Put };
    public enum PriceType { Price, Δ, Vega, Θ, Γ, ρ };
    /// <summary>
    /// Summary description for BlackSholes.
    /// </summary>
    public class BlackScholes
    {
        public static double Price(OptionType optionType, double Fwd, double K,
            double T, double r, double σ, PriceType priceType = PriceType.Price)
        {
            double std = σ * Math.Sqrt(T);
            double DF = Math.Exp(-r * T);

            double d1 = (Math.Log(Fwd / K) + 0.5*std*std ) / std;
            double d2 = d1 - std;
            
            switch(priceType)
            {
                case PriceType.Price:
                    if (optionType == OptionType.Call)
                        return DF * ( Fwd * N(d1) - K *  N(d2) );
                    else if (optionType == OptionType.Put)
                        return DF * ( K * N(-d2) - Fwd * N(-d1) );
                    break;
                case PriceType.Δ:
                    return DF * Fwd * N(d1);
                case PriceType.Vega:
                    return DF * Fwd * Math.Sqrt(T) * Nprime(d1);
                case PriceType.Γ:
                    return 1 / ( DF * Fwd * std ) * Nprime(d1);
            }

            throw new Exception();
        }

        public static double Price(OptionType optionType, Equity equity, double K,
            double T, double r, PriceType priceType = PriceType.Price)
        {
            var impliedVol = equity.sigma;
            return Price(optionType, equity.spot, K, T, r, impliedVol, priceType);
        }

        public static double ImpliedVol(OptionType optionType, double price, double Fwd, double K,
            double T, double r,  double accuracy = 1e-4 )
        {
            Func<double,double> pricer = (v) => Price(optionType, Fwd, K, T, r, v) - price;
            Func<double, double> vega  = (v) => Price(optionType, Fwd, K, T, r, v, PriceType.Vega);

            var solve = MathNet.Numerics.RootFinding.NewtonRaphson.FindRootNearGuess(
                pricer, vega, 0.2, 0, 1, accuracy: accuracy);
            return solve;
        }

        public static double CND2(double X)
        {
            return new Normal().CumulativeDistribution(X);
        }

        public static double Nprime(double X)
        {
            const double piconst = 0.398942280401433; // 1.0 / Math.Sqrt(2 * Math.PI)
            return piconst * Math.Exp(-0.5 * X * X);
        }

        public static double N(double X)
        {
            const double a1 = 0.31938153;
            const double a2 = -0.356563782;
            const double a3 = 1.781477937;
            const double a4 = -1.821255978;
            const double a5 = 1.330274429;
            double L = Math.Abs(X);
            double K = 1.0 / (1.0 + 0.2316419 * L);
            const double piconst = 0.398942280401433; // 1.0 / Math.Sqrt(2 * Math.PI)
            double dCND = 1.0 - piconst *
                Math.Exp(-L * L / 2.0) * K * (a1 + K * (a2 + K * (a3 +
                K * (a4 + a5 * K))));

            if (X < 0)
            {
                return 1.0 - dCND;
            }
            else
            {
                return dCND;
            }
        }
    }
}

