namespace USO.Dto
{
    using System;
    using System.Collections.Generic;
    using USO.Domain.Extensions;

    public static class Validation
    {
        public static Validation<TServiceResult> Validate<TServiceResult>(Func<bool> condition, string parameterName, string errorMessage) where TServiceResult : ServiceResultBase
        {
            return new Validation<TServiceResult>(condition, parameterName, errorMessage);
        }
    }

    public class Validation<TServiceResult> where TServiceResult : ServiceResultBase
    {
        public Validation(Func<bool> condition, string parameterName, string errorMessage)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");

            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException("parameterName");
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentNullException("errorMessage");

            Condition = condition;
            ParameterName = parameterName;
            ErrorMessage = errorMessage;
        }

        protected Func<bool> Condition
        {
            get;
            private set;
        }

        protected string ParameterName
        {
            get;
            private set;
        }

        protected string ErrorMessage
        {
            get;
            private set;
        }

        public Validation<TServiceResult> Or(Func<bool> condition, string parameterName, string errorMessage)
        {
            return new OrValidation(this, condition, parameterName, errorMessage);
        }

        public Validation<TServiceResult> And(Func<bool> condition, string parameterName, string errorMessage)
        {
            return new AndValidation(this, condition, parameterName, errorMessage);
        }

        public TServiceResult Result()
        {
            IList<RuleViolation> ruleViolations = Validate();

            return (TServiceResult)Activator.CreateInstance(typeof(TServiceResult), new object[] { ruleViolations });
        }

        protected virtual IList<RuleViolation> Validate()
        {
            IList<RuleViolation> violations = new List<RuleViolation>();

            if (Condition())
            {
                violations.Add(new RuleViolation(ParameterName, ErrorMessage));
            }

            return violations;
        }

        private abstract class CompositeValidation : Validation<TServiceResult>
        {
            protected CompositeValidation(Validation<TServiceResult> innerValidation, Func<bool> condition, string parameterName, string errorMessage)
                : base(condition, parameterName, errorMessage)
            {
                InnerValidation = innerValidation;
            }

            protected Validation<TServiceResult> InnerValidation
            {
                get;
                private set;
            }
        }

        private sealed class OrValidation : CompositeValidation
        {
            public OrValidation(Validation<TServiceResult> innerValidation, Func<bool> condition, string parameterName, string errorMessage)
                : base(innerValidation, condition, parameterName, errorMessage)
            {
            }

            protected override IList<RuleViolation> Validate()
            {
                IList<RuleViolation> violations = InnerValidation.Validate();

                if (violations.IsEmpty())
                {
                    violations.AddRange(base.Validate());
                }

                return violations;
            }
        }

        private sealed class AndValidation : CompositeValidation
        {
            public AndValidation(Validation<TServiceResult> innerValidation, Func<bool> condition, string parameterName, string errorMessage)
                : base(innerValidation, condition, parameterName, errorMessage)
            {
            }

            protected override IList<RuleViolation> Validate()
            {
                IList<RuleViolation> violations = InnerValidation.Validate();

                violations.AddRange(base.Validate());

                return violations;
            }
        }
    }
}