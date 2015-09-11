
namespace USO.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PagedResult<T>
    {
        public PagedResult(IEnumerable<T> result, int total)
        {
            if (result == null)
                throw new ArgumentNullException("result", string.Format("\"{0}\" cannot be null", "result"));
            if (total < 0)
                throw new ArgumentNullException("total", string.Format("\"{0}\" cannot be negative", "total"));

            Result = result;//.ToList();// new List<T>(result);// new ReadOnlyCollection<T>(new List<T>(result));
            Total = total;
        }


        public PagedResult()
            : this(new List<T>(), 0)
        {
        }

        public IEnumerable<T> Result
        {
            get;
            private set;
        }

        public int Total
        {
            get;
            private set;
        }

        public bool IsEmpty
        {

            get
            {
                return (Result.Count() == 0) || (Total == 0);
            }
        }
    }
}
