
namespace USO.Utility
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Web;

    public class InterfaceScanner
    {
        private static Assembly appDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return null;
        }

        public static string[] GetClassesBasedOnTypeInSiteDir(Type assemblyType)
        {
            return GetClassesBasedOnTypeInSiteDir(assemblyType, HttpContext.Current.Server.MapPath("~"));
        }

        public static string[] GetClassesBasedOnTypeInSiteDir(Type assemblyType, string path)
        {
            ArrayList list = new ArrayList();
            if (!Directory.Exists(path))
            {
                return (string[])list.ToArray(typeof(string));
            }
            LocalLoader loader = new LocalLoader(path);
            string[] files = Directory.GetFiles(path + @"\bin", "*.dll");
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    if (!new FileInfo(files[i]).Name.StartsWith("McLicenseVerify"))
                    {
                        loader.LoadAssembly(files[i]);
                    }
                }
                catch (Exception)
                {
                }
            }
            string[] subclasses = loader.GetSubclasses(assemblyType.ToString());
            loader.Unload();
            return subclasses;
        }
    }
}
