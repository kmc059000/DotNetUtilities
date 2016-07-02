using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumMapper
{
    public abstract class EnumMapper<TEnum1, TEnum2> 
        where TEnum1 : struct 
        where TEnum2 : struct 
    {
        private static readonly IDictionary<TEnum1, TEnum2> Map1 = new Dictionary<TEnum1, TEnum2>(); 
        private static readonly IDictionary<TEnum2, TEnum1> Map2 = new Dictionary<TEnum2, TEnum1>();

        protected static void AutoMap()
        {
            var values1 = Enum.GetValues(typeof (TEnum1)).Cast<TEnum1>().ToList();
            var values2 = Enum.GetValues(typeof (TEnum2)).Cast<TEnum2>().ToList();

            var matches =
                from a in values1
                from b in values2
                where a.ToString() == b.ToString()
                select new {a, b};

            foreach (var match in matches)
            {
                CreateMap(match.a, match.b);
            }
        }

        protected static void CreateMap(TEnum1 e1, TEnum2 e2)
        {
            Map1.Add(e1, e2);
            Map2.Add(e2, e1);
        }

        public TEnum2 Map(TEnum1 source)
        {
            return Map1[source];
        }

        public TEnum1 Map(TEnum2 source)
        {
            return Map2[source];
        }

        public TEnum2? TryMap(TEnum1? source)
        {
            return source.HasValue
                ? Map1.ContainsKey(source.Value)
                    ? Map1[source.Value]
                    : (TEnum2?) null
                : null;
        }

        public TEnum1? TryMap(TEnum2? source)
        {
            return source.HasValue
                ? Map2.ContainsKey(source.Value)
                    ? Map2[source.Value]
                    : (TEnum1?) null
                : null;
        }
    }
}
