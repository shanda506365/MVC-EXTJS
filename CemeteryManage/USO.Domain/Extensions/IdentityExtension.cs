
namespace USO.Domain.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class IdentityExtension
    {
        public static long[] ToLongArray(this string instance, string separator)
        {
            var result = new List<long>();
            if (string.IsNullOrWhiteSpace(instance))
                return result.ToArray();

            var strItems = instance.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var strItem in strItems)
            {
                long item = -1;
                if (long.TryParse(strItem, out item))
                    result.Add(item);
            }

            return result.ToArray();
        }


        public static Tuple<long, int>[] ToIdAndYearArray(this string instance, string separator)
        {
            var result = new List<Tuple<long, int>>();
            if (string.IsNullOrWhiteSpace(instance))
                return result.ToArray();

            var strItems = instance.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var strItem in strItems)
            {
                var idAndYear = strItem.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (idAndYear.Length == 2)
                {
                    long id;
                    int year;
                    if (long.TryParse(idAndYear[0], out id) && int.TryParse(idAndYear[1], out year))
                        result.Add(Tuple.Create(id,year));
                }
            }

            return result.ToArray();
        }


        public static Tuple<long, int>[] ToIdAndYearArray(this string[] instance)
        {
            var result = new List<Tuple<long, int>>();
            if (instance == null || instance.Length==0)
                return result.ToArray();

            foreach (var strItem in instance)
            {
                var idAndYear = strItem.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (idAndYear.Length == 2)
                {
                    long id;
                    int year;
                    if (long.TryParse(idAndYear[0], out id) && int.TryParse(idAndYear[1], out year))
                        result.Add(Tuple.Create(id, year));
                }
            }

            return result.ToArray();
        }


        public static string[] ToIdAndYearStrings(this Tuple<long, int>[] instance)
        {
            var result = new List<string>();
            if (instance == null || instance.Length == 0)
                return result.ToArray();

            foreach (var item in instance)
            {
                if (item.Item1>0 && item.Item2>0)
                {
                    var idAndYear = string.Format("{0}_{1}", item.Item1, item.Item2);
                    if (!result.Contains(idAndYear))
                        result.Add(idAndYear);
                }
            }

            return result.ToArray();
        }


        public static string ToIdAndYearString(this Tuple<long, int>[] instance, string separator)
        {
            if (instance == null || instance.Length == 0)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(separator))
                separator = ",";

            var idAndYears = instance.ToIdAndYearStrings();

            return string.Join(separator, idAndYears);
        }


        public static Tuple<int, long, int>[] ToHashAndIdAndYearArray(this string instance, string separator)
        {
            var result = new List<Tuple<int, long, int>>();
            if (string.IsNullOrWhiteSpace(instance))
                return result.ToArray();

            var strItems = instance.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var strItem in strItems)
            {
                var hashAndIdAndYear = strItem.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (hashAndIdAndYear.Length == 3)
                {
                    int hash;
                    long id;
                    int year;
                    if (int.TryParse(hashAndIdAndYear[0], out hash) && long.TryParse(hashAndIdAndYear[1], out id) && int.TryParse(hashAndIdAndYear[2], out year))
                        result.Add(Tuple.Create(hash, id, year));
                }
            }

            return result.ToArray();
        }
       
    }
}
