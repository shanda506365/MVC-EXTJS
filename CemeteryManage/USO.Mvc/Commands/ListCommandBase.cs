namespace USO.Mvc.Commands
{
    using System.Linq;
    using System.Web.Mvc;
    using USO.Domain;
    using Telerik.Web.Mvc;

    public abstract class ListCommandBase
    {
        private readonly string _prefix = string.Empty;

        public ListCommandBase()
        {
            
        }

        //ViewModel; 
        public ListCommandBase(GridCommand command, ControllerBase controller)
            : this(command, controller,"")
        { 
        }

        public ListCommandBase(GridCommand command, ControllerBase controller, string prefix)
        {
            Check.Argument.IsNotNull(command, "command");
            Check.Argument.IsNotNull(controller, "controller");

            _prefix = prefix;

            GridCommand = command;
            if (GridCommand.PageSize <= 0) GridCommand.PageSize = 50;
            if (GridCommand.Page <= 0) GridCommand.Page = 1;

            BindCommand(controller);
        }

        public GridCommand GridCommand { get; set; }

        public virtual string Prefix(string field)
        {
            return string.IsNullOrWhiteSpace(_prefix) ? field : string.Format("{0}.{1}", _prefix, field);
        }

        /// <summary>
        /// 解析并绑定命令属性
        /// </summary>
        /// <param name="controller"></param>
        public abstract void BindCommand(ControllerBase controller);

        /// <summary>
        /// 运用命令属性到查询
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<T> ApplyCommand<T>(IQueryable<T> query)
        {
            return query;
        }

        /// <summary>
        /// 运用Grid传递来的查询参数
        /// </summary>
        public virtual IQueryable<T> ApplyGridFilterDescriptors<T>(IQueryable<T> query)
        {
            if (GridCommand.FilterDescriptors.Any())
            {
                query = query.Where(ExpressionBuilder.Expression<T>(GridCommand.FilterDescriptors));
            }

            return query;
        }

        /// <summary>
        /// 运用Grid传递来的排序参数
        /// </summary>
        public virtual IQueryable<T> ApplyGridSortDescriptors<T>(IQueryable<T> query)
        {
            return query;
        }

        /// <summary>
        /// 运用Grid传递来的分页参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IQueryable<T> ApplyGridPaging<T>(IQueryable<T> query)
        {
            // ... and paging
            if (GridCommand.PageSize > 0)
            {
                query = query.Skip((GridCommand.Page - 1) * GridCommand.PageSize);
            }

            query = query.Take(GridCommand.PageSize);

            return query;
        }
    }
}