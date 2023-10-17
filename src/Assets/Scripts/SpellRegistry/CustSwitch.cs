using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRegistry_IGB200
{
    public static class SwitchCase_
    {

        public static T FirstCase<T>(Func<T, bool> predicate, IEnumerable<T> checkAgainst)
        {
            return checkAgainst.First(predicate);
        }

        public static IEnumerable<T> AllCase<T>(Func<T,bool> predicate, IEnumerable<T> checkAgainst)
        {
            return checkAgainst.Where(predicate);
        }

    }
}
