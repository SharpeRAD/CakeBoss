#region Using Statements
    using System;
    using System.Collections.Generic;
#endregion



namespace CakeBoss.Host
{
    public static class HashSetExtensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    set.Add(item);
                }
            }
        }
    }
}
