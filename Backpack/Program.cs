using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpack
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadLines("data.txt").Select(l =>
            {
                try
                {
                    return (decimal?) decimal.Parse(l
                            .Replace(",", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator)
                            .Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
                        CultureInfo.InvariantCulture);
                }
                catch
                {
                    return null;
                }
            }).Where(x => x.HasValue).Select(x => x.Value);

            var sum = input.First();
            var data = input.Skip(1).Where(x=>x<=sum).ToList();
            var dataDesc = data.OrderByDescending(x => x).ToList();

            var sw = new Stopwatch();

            for (int i = 1; i <= data.Count; ++i)
            {
                sw.Restart();
                var res = GetMaxSum(sum, data.Take(i)).ToList();
                sw.Stop();

                Console.WriteLine("Elements: {0}; Time: {1}", i, sw.Elapsed);
                Console.WriteLine("Max Sum: {0}/{2}. Data: {1}", res.Sum(), string.Join(" ; ", res), sum);
                Console.WriteLine();

                sw.Restart();
                var res2 = GetMaxSum(sum, dataDesc.Take(i)).ToList();
                sw.Stop();

                Console.WriteLine("Max elements: {0}; Time: {1}", i, sw.Elapsed);
                Console.WriteLine("Max Sum: {0}/{2}. Data: {1}", res2.Sum(), string.Join(" ; ", res2), sum);
                Console.WriteLine("------------------------------------");

                if (res.Sum() >= sum || res2.Sum() >= sum) break;
            }

            Console.WriteLine("Completed!");

            Console.ReadLine();
        }

        public static IEnumerable<decimal> GetMaxSum(decimal max, IEnumerable<decimal> data)
        {
            if (!data.Any()) return new decimal[0];

            var elem = data.First();

            if (elem > max) return GetMaxSum(max, data.Skip(1));

            var t1 = Task.Run(() => GetMaxSum(max, data.Skip(1)));
            var t2 = Task.Run(() => GetMaxSum(max - elem, data.Skip(1)));

            Task.WaitAll(t1, t2);
            var noTake = t1.Result;
            var take = t2.Result;

            if (noTake.Sum() >= take.Sum() + elem)
            {
                return noTake;
            }

            return new[] {elem}.AsEnumerable().Union(take);
        }
    }
}