using Moq;
using System.Linq.Expressions;

namespace PDS.VYF.Services.Tests.Mocks
{
    /// <summary>
    /// The Base class of Mock.
    /// </summary>
    /// <typeparam name="T">Any class.</typeparam>
    /// <seealso cref="Moq.Mock&lt;T&gt;" />
    public abstract class MockBase<T>
        : Mock<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockBase{T}" /> class.
        /// </summary>
        /// <param name="isSetupDefault">if set to <c>true</c> [is setup default].</param>
        protected MockBase(bool isSetupDefault = true)
            : base(MockBehavior.Strict)
        {
            if (isSetupDefault)
            {
                this.SetupDefault();
            }
        }

        /// <summary>
        /// Setups the default.
        /// </summary>
        public abstract void SetupDefault();

        public Expression<Action<T>> CallTo(Expression<Action<T>> expression)
        {
            return expression;
        }

        public Expression<Func<T, TResult>> CallTo<TResult>(Expression<Func<T, TResult>> expression)
        {
            return expression;
        }
    }
}
