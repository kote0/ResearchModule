using ResearchModule.Components.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResearchModule.Components.Models
{
    public class Result : IResult
    {
        public bool Succeeded
        {
            get { return Error.Count() == 0 ? true : false; }
        }

        public bool Failed
        {
            get { return !Succeeded; }
        }

        public List<string> Error { get; private set; }

        #region ctor

        public Result()
        {
            Error = new List<string>();
        }

        #endregion

        #region methods

        public IResult Set(IResult result)
        {
            return Set(result.Error);
        }

        public IResult Set(string str, params object[] obj)
        {
            return Set(string.Format(str, obj));
        }

        public IResult Set(string str)
        {
            Error.Add(str);
            return this;
        }

        public IResult Set(IEnumerable<string> list)
        {
            if (list.Count() != 0)
                Error.AddRange(list);
            return this;
        }

        #endregion
    }
}
