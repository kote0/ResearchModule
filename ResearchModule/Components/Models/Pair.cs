using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public class Pair<TFirst, TSecond>
    {
        private TFirst _first;
        private TSecond _second;


        ///<summary>
        ///</summary>
        ///<param name="first"></param>
        ///<param name="second"></param>
        public Pair(TFirst first, TSecond second)
        {
            _first = first;
            _second = second;
        }


        ///<summary>
        ///</summary>
        public Pair()
        {
        }

        ///<summary>
        /// Значение первого элемента
        ///</summary>
        public TFirst First
        {
            get { return _first; }
            set { _first = value; }
        }

        ///<summary>
        /// Значение второго элемента
        ///</summary>
        public TSecond Second
        {
            get { return _second; }
            set { _second = value; }
        }
    }
}
