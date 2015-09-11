
namespace USO.Mvc.ViewModels
{
    using System.Collections.Generic;

    /// <summary>
    /// EXT Grid 的数据源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtGridViewModel<T> where T : class
    {
        public bool Success { get; set; }

        public List<T> Items { get; set; }

        public int TotalCount { get; set; }
    }

    public class ExtGridEditResultModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
