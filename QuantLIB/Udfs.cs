using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;

namespace CSharpUdfs
{
    public class TestUdfs
    {
        public static double[] Interpolate2(double[] xs, double[] ys, double[] newxs, string type)
        {
            IInterpolation interpolator;
            if (type == "cubic")
                interpolator = MathNet.Numerics.Interpolate.CubicSpline(xs, ys);
            else if (type == "linear")
                interpolator = MathNet.Numerics.Interpolate.Linear(xs, ys);
            else if (type == "polynomial")
                interpolator = MathNet.Numerics.Interpolate.Polynomial(xs, ys);
            else throw new NotImplementedException();
            return newxs.Select(interpolator.Interpolate).ToArray();
        }

        public enum InterpolationType { Flat, Linear, None }

        public static double[] Interpolate(double[] xs, double[] ys, double[] newxs, string interpolationType)
        {
            var enumType = (InterpolationType)Enum.Parse(typeof(InterpolationType), interpolationType);
            return Interpolate(xs, ys, newxs, enumType);
        }

        private static double[] Interpolate(double[] xs, double[] ys, double[] newxs, InterpolationType interpolationType)
        {
            var results = newxs
                .Select(x =>
                {
                    var i = 0;
                    Func<int, double> interpolate = j => (ys[j + 1] - ys[j]) / (xs[j + 1] - xs[j]) * (x - xs[j]) + ys[j];

                    if (x < xs[0])
                    {
                        if (interpolationType == InterpolationType.Linear)
                            return interpolate(0);
                        else if (interpolationType == InterpolationType.Flat)
                            return ys[0];
                        else
                            return double.NaN;
                    }
                    if (x > xs[xs.Count() - 1])
                    {
                        if (interpolationType == InterpolationType.Linear)
                            return interpolate(xs.Count() - 2);
                        else if (interpolationType == InterpolationType.Flat)
                            return ys[xs.Count() - 1];
                        else
                            return double.NaN;
                    }

                    for (i = 0; i < xs.Count(); i++)
                    {
                        if (xs[i] <= x && xs[i + 1] >= x) break;
                    }

                    return interpolate(i);
                })
                .ToList();

            return results.ToArray();
        }

        public static double OnePlusOne(double x, double y)
        {

            return x + y*y;
        }
        public static string BuildQuery3(object obj)
        {
            var conditions = obj as object[,];
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < conditions.GetLength(0); i++)
            {
//                if (conditions[i, 1] is ExcelDna.Integration.ExcelEmpty)
//                    continue;
                var value = conditions[i, 1].ToString();
                dict[conditions[i, 0].ToString()] = value;
            }
            return BuildQueryFromDictionary(dict);
        }

        /// <summary>
        /// Given a list of fields and conditions, build the corresponding SQL predicate
        /// Fund => BMEA,BMCA
        /// Ticker => %SPX,%VIX
        /// will return "Fund IN ('BMEA','BMCA') AND ( Ticker LIKE '%SPX' OR Ticker LIKE '%VIX')"
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        static string BuildQueryFromDictionary(Dictionary<string, string> conditions)
        {
            if (!conditions.Any())
                return "1=1";
            return string.Join(" AND ", conditions
                .Select(kvp => { 
                    var transformValues = kvp.Value.Split(',').Select(r => "'" + r + "'");
                    if (transformValues.Any(v => v.Contains("%")))
                        return "(" + string.Join(" OR ", transformValues.Select(v => kvp.Key + " LIKE " + v)) + ")";
                    return string.Format("{0} in ({1})", kvp.Key, string.Join(",", transformValues));
                })
                );
        }
    }


}
