namespace USO.Dto
{
    using System.Diagnostics;
    using System;

    public class RuleViolation
    {
        [DebuggerStepThrough]
        public RuleViolation(string parameterName, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");

            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentNullException("errorMessage");



            ParameterName = parameterName;
            ErrorMessage = errorMessage;
        }

        public string ParameterName
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}

