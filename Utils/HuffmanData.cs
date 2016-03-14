using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JHLib.Utils
{
    public class HuffmanData
    {
        private static Dictionary<char,int> CalculateFrequencies(string message)
        {
            var freqs = new Dictionary<char, int>();
            foreach (var c in message)
            {
                if (!freqs.ContainsKey(c))
                    freqs[c] = 0;

                freqs[c]++;
            };

            return freqs
                .OrderByDescending(f => f.Value)
                .ToDictionary(f => f.Key, f => f.Value);
        }
        
        public static HuffmanData FromString(string message)
        {
            var freqs = CalculateFrequencies(message);

            throw new NotImplementedException();
        }

        public string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
