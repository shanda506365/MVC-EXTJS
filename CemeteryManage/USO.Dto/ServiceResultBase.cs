namespace USO.Dto
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System;

    public abstract class ServiceResultBase
    {
        [DebuggerStepThrough]
        protected ServiceResultBase(IEnumerable<RuleViolation> ruleViolations)
        {
            if (ruleViolations == null)
                throw new ArgumentNullException("ruleViolations");

            RuleViolations = new List<RuleViolation>(ruleViolations);
        }

        public IList<RuleViolation> RuleViolations
        {
            get;
            private set;
        }
    }
}