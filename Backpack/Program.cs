using System;
using System.Collections.Generic;
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
                    return (decimal?) decimal.Parse(l);
                }
                catch
                {
                    return null;
                }
            }).Where(x => x.HasValue).Select(x => x.Value);

            var sum = input.First();
            var data = input.Skip(1).ToList();
            
            var res = GetMaxSum(sum, data);

            Console.WriteLine("Max Sum: {0}({2}). Data:{1}", res.Sum(), string.Join(" ; ",res), sum);
            Console.ReadLine();
        }

        public static IEnumerable<decimal> GetMaxSum(decimal max, IEnumerable<decimal> data)
        {
            if(!data.Any()) return new decimal[0];

            var elem = data.First();

            if (elem > max) return GetMaxSum(max, data.Skip(1));

            var noTake = GetMaxSum(max, data.Skip(1));
            var take = GetMaxSum(max - elem, data.Skip(1));

            if (noTake.Sum() >= take.Sum() + elem)
            {
                return noTake;
            }

            return new[] {elem}.AsEnumerable().Union(take);

        }
    }
}
