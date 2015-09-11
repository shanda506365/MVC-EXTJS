
namespace USO.Dto
{
    using System.Collections.Generic;
    using System;


    public class ServiceResult<T> : ServiceResultBase
    {
        public ServiceResult()
            : this(new List<RuleViolation>())
        {
        }

        public ServiceResult(T data)
            : this()
        {
            if (data == null)
                throw new ArgumentNullException("data");

            ResultData = data;
        }

        public ServiceResult(IEnumerable<RuleViolation> ruleViolations)
            : base(ruleViolations)
        {
        }

        public T ResultData { get; set; }
    }
}
