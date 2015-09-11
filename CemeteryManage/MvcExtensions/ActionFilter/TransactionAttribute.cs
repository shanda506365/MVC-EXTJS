using System;
using System.Web.Mvc;
using System.Web;
using System.Transactions;

namespace MvcExtensions
{
    /// <summary>
    /// 这是一个控制Action事务的一个Attribute，若一个Action需要事务支持，只需在Action方法前加上[Transaction]Attribute即可，以下是几点说明：
    /// 1、该Attribute可以控制TransactionScope和IsolationLevel；
    /// 2、TransactionScope可以通过named parameter来控制，可选项是以下几种：
    ///		a、TransactionScopeOption.Required：该范围需要一个事务。如果已经存在环境事务，则使用该环境事务。否则，在进入范围之前创建新的事务。【这是默认值】。 
    ///		b、TransactionScopeOption.RequiresNew：总是为该范围创建新事务。
    ///		c、TransactionScopeOption.Suppress：环境事务上下文在创建范围时被取消。范围中的所有操作都在无环境事务上下文的情况下完成。
    ///	3、IsolationLevel可以通过positonal parameter来控制，就是通过constructor来传递，可选项是以下几种：
    ///		a、IsolationLevel.ReadUncommitted：允许脏读取，但不允许更新丢失。读操作不加任何锁，写操作加排写锁到事务结束。也就是T1在写时，T2能够读T1尚未提交的数据。
    ///		b、IsolationLevel.ReadCommitted：允许不可重复读取，但不允许脏读取。读操作加读锁，读完立即释放，写操作加排写锁到事务结束。
    ///			比如，T1有三个动作读1、写、读2，在读1完成后，就允许T2写，这样在读2时有可能会读到和读2不一样的数据，因此也叫NonRepeatable Read。
    ///		c、IsolationLevel.RepeatableRead：禁止不可重复读取和脏读取，但是有时可能出现幻影数据。读操作加读锁到事务结束，写操作加排写锁到事务结束。
    ///			仍然用上面的例子，在读1完成后，仍然不允许T2写，这样在读2时有可能会读到和读2不一样的数据，因此也叫NonRepeatable Read。但是在读1完成时，T2可以加新数据，因此读1返回的数据和读2是一样的，但是读2有可能多一些新数据。
    ///		d、IsolationLevel.Serializable：提供严格的事务隔离。它要求事务序列化执行，事务只能一个接着一个地执行，但不能并发执行。
    ///		e、IsolationLevel.Snapshot：指定事务中任何语句读取的数据都将是在事务开始时便存在的数据的事务上一致的版本。事务只能识别在其开始之前提交的数据修改。
    ///			在当前事务中执行的语句将看不到在当前事务开始以后由其他事务所做的数据修改。其效果就好像事务中的语句获得了已提交数据的快照，因为该数据在事务开始时就存在。
    ///			这是SQL SERVER 2005的新功能，具体效果和使用场合不清楚。
    ///		f、IsolationLevel.Chaos：现在还不清楚。
    ///		g、IsolationLevel.Unspecified：废弃不用
    ///	4、如果不指明任何属性参数，即仅[Transaction]，将默认为TransactionScopeOption.Required，和IsolationLevel.ReadCommitted。其实其它可选项使用的场合也不多。
    ///	5、用法指南：
    ///		[Transaction]
    ///		[Transaction(IsolationLevel.ReadUncommitted, TransactionScope = TransactionScopeOption.RequiresNew)]
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TransactionAttribute : ActionFilterAttribute
    {
        private const string _ef5_transactionstring = "ef5_transaction";

        private readonly IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;
        /// <summary>
        /// IsolationLevel
        /// </summary>
        /// <param name="isolationLevel"></param>
        public TransactionAttribute(IsolationLevel isolationLevel) 
        {
            _isolationLevel = isolationLevel;
        }
        /// <summary>
        ///  IsolationLevel
        /// </summary>
        public TransactionAttribute()	
            : this(IsolationLevel.ReadCommitted)
        { }


        private TransactionScopeOption _transactionScope = TransactionScopeOption.Required;
        /// <summary>
        /// TransactionScopeOption
        /// </summary>
        public TransactionScopeOption TransactionScope 
        {
            get { return _transactionScope; }
            set { _transactionScope = value; }
        }

        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!HttpContext.Current.Items.Contains(_ef5_transactionstring))
                HttpContext.Current.Items[_ef5_transactionstring] = new TransactionScope(_transactionScope, new TransactionOptions { IsolationLevel = _isolationLevel });
        }

        /// <summary>
        /// Action执行后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (HttpContext.Current.Items.Contains(_ef5_transactionstring))
            {
                var scope = HttpContext.Current.Items[_ef5_transactionstring] as TransactionScope;
                if (filterContext.Exception == null)
                    scope.Complete();
                scope.Dispose();
            }
        }
    }
}
