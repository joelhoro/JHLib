using System;
using System.Collections.Generic;

namespace JHLib.QuantLIB.Model
{
    public class SecurityStore
    {
        private static Dictionary<string, object> _store = new Dictionary<string,object> {

            { "MSFT",       new Equity(100,0.35)         },
            { "GE",         new Equity(58,0.2)           },

        };

        public static Equity GetEquity(string tag)
        {
            Object obj;
            _store.TryGetValue(tag, out obj);
            return obj as Equity;
        }
    }
}
