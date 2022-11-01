using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_CSS.Helper
{
    public class RouteParamManager
    {
        public static string Concat(string route, char separator, params string[] param)
        {
            var split = route.Split(separator);
            if (split.Length == 1)
                return split[0];

            if (param.Length+1 < split.Length)
                throw new IndexOutOfRangeException("Not enough parameters in input array to merge string");

            string res = "";
            for (int i = 0; i < split.Length-1;i++)
                res += split[i] + param[i];
            res += split[split.Length - 1];

            return res;
        }
    }
}
