
namespace USO.Utility
{
    using System;
    using System.Reflection;

    public class LocalLoader : MarshalByRefObject
    {
        private AppDomain appDomain;
        private RemoteLoader remoteLoader;

        public LocalLoader(string pluginDirectory)
        {
            AppDomainSetup info = new AppDomainSetup();
            info.ApplicationName = string.Format("Plugins-{0}", Guid.NewGuid().ToString());
            info.ApplicationBase = pluginDirectory;
            info.PrivateBinPath = "bin";
            this.appDomain = AppDomain.CreateDomain("Plugins", null, info);
            this.remoteLoader = (RemoteLoader)this.appDomain.CreateInstanceAndUnwrap(Assembly.GetExecutingAssembly().FullName, "USO.Utility.RemoteLoader");
        }

        public object CallStaticMethod(string typeName, string methodName, object[] methodParams)
        {
            return this.remoteLoader.CallStaticMethod(typeName, methodName, methodParams);
        }

        public MarshalByRefObject CreateInstance(string typeName, BindingFlags bindingFlags, object[] constructorParams)
        {
            return this.remoteLoader.CreateInstance(typeName, bindingFlags, constructorParams);
        }

        public object GetStaticPropertyValue(string typeName, string propertyName)
        {
            return this.remoteLoader.GetStaticPropertyValue(typeName, propertyName);
        }

        public string[] GetSubclasses(string baseClass)
        {
            return this.remoteLoader.GetSubclasses(baseClass);
        }

        public void LoadAssembly(string filename)
        {
            this.remoteLoader.LoadAssembly(filename);
        }

        public bool ManagesType(string typeName)
        {
            return this.remoteLoader.ManagesType(typeName);
        }

        public void Unload()
        {
            AppDomain.Unload(this.appDomain);
            this.appDomain = null;
        }

        public string[] Assemblies
        {
            get
            {
                return this.remoteLoader.GetAssemblies();
            }
        }

        public string[] Types
        {
            get
            {
                return this.remoteLoader.GetTypes();
            }
        }
    }
}
