namespace PDS.ViewYourFunding.Services.Models
{
    /// <summary>
    /// The component parts of a where clause part (e.g A=B).
    /// </summary>
    public class EvaluatedExpressionComponent
    {
        /// <summary>
        /// Gets or sets the original expression (e.g. A=B).
        /// </summary>
        public string OriginalExpression { get; set; }

        /// <summary>
        /// Gets or sets the left hand component (e.g. A).
        /// </summary>
        public string LeftHandComponent { get; set; }

        /// <summary>
        /// Gets or sets the right component (e.g. B).
        /// </summary>
        public string RightHandComponent { get; set; }

        /// <summary>
        /// Gets or sets the left hand evaluated result (e.g. A in the examples above - or in the case the left hand component was @YearType - this might be 'Academic').
        /// </summary>
        public object LeftHandComponentEvaluatedResult { get; set; }

        /// <summary>
        /// Gets or sets the right hand evaluated result (e.g. A in the examples above - or in the case the left hand component was @YearType - this might be 'Academic').
        /// </summary>
        public object RightHandComponentEvaluatedResult { get; set; }

        /// <summary>
        /// Gets or sets the operator (e.g. '=').
        /// </summary>
        public char? Operator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is equals to (if not it is 'not equals to' - this on its own does not show the operator to use).
        /// </summary>
        public bool? EqualsTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the expression has been evaluated yet.
        /// </summary>
        public bool Evaluated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the right hand expression has been evaluated yet.
        /// </summary>
        public bool RightHandEvaluated { get; set; }

        /// <summary>
        /// Gets or sets a value containing the result of the evaluated expression.
        /// </summary>
        public bool? ComparisonResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the type is a selector or not.
        /// </summary>
        public bool IsSelector { get; set; }

        /// <summary>
        /// Gets or sets the context (optional).
        /// </summary>
        public object Context { get; set; }

        /// <summary>
        /// Clone a component (deep copy).
        /// </summary>
        /// <returns>The clone.</returns>
        public EvaluatedExpressionComponent Clone()
        {
            return new EvaluatedExpressionComponent
            {
                OriginalExpression = OriginalExpression,
                EqualsTo = EqualsTo,
                Evaluated = Evaluated,
                LeftHandComponent = LeftHandComponent,
                LeftHandComponentEvaluatedResult = LeftHandComponentEvaluatedResult,
                Operator = Operator,
                ComparisonResult = ComparisonResult,
                RightHandComponent = RightHandComponent,
                RightHandComponentEvaluatedResult = RightHandComponentEvaluatedResult,
                RightHandEvaluated = RightHandEvaluated,
                IsSelector = IsSelector,
                Context = Context
            };
        }
    }
}