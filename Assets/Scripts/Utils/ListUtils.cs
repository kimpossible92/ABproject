using System.Collections.Generic;

namespace SnakeMaze.Utils
{
    public class ListUtils
    {
        /// <summary>
        /// Concats two Lists with elements of data T.
        /// </summary>
        /// <param name="firstList"></param>
        /// <param name="secondList"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> Concat<T>(List<T> firstList, List<T> secondList)
        {
            foreach (T t in secondList)
            {
                firstList.Add(t);
            }

            return firstList;
        }
    }
}