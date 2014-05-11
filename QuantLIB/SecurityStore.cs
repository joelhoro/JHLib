using System;
using System.Collections.Generic;

namespace JHLib.QuantLIB
{
    public class SecurityStore
    {
        private static Dictionary<string, object> _store = new Dictionary<string,object> {

            { "MSFT",       new Equity { spot = 100, sigma = 0.35 }         },
            { "GE",         new Equity { spot = 58, sigma = 0.2 }           },

        };

        public static Equity GetEquity(string tag)
        {
            Object obj;
            _store.TryGetValue(tag, out obj);
            return obj as Equity;
        }
    }
}
