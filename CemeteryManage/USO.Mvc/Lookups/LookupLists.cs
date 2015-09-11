
namespace USO.Mvc.Lookup
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using USO.Core;

    public class LookupLists : IDependency
    {

        public static LookupLists Current
        {
            get { return DependencyResolver.Current.GetService<LookupLists>(); }
        }



       
    }
}
