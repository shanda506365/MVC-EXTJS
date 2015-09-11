
namespace USO.Utility
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;

    public class RemoteLoader : MarshalByRefObject
    {
        protected ArrayList assemblyList = new ArrayList();
        protected ArrayList typeList = new ArrayList();

        public object CallStaticMethod(string typeName, string methodName, object[] methodParams)
        {
            Type typeByName = this.GetTypeByName(typeName);
            if (typeByName == null)
            {
                throw new ArgumentException("Cannot find a type of name " + typeName + " within the plugins or the common library.");
            }
            return typeByName.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static).Invoke(null, BindingFlags.Public | BindingFlags.Static, null, methodParams, null);
        }

        public MarshalByRefObject CreateInstance(string typeName, BindingFlags bindingFlags, object[] constructorParams)
        {
            Assembly assembly = null;
            foreach (Assembly assembly2 in this.assemblyList)
            {
                if (assembly2.GetType(typeName) != null)
                {
                    assembly = assembly2;
                }
            }
            if (assembly == null)
            {
                throw new InvalidOperationException("Could not find owning assembly for type " + typeName);
            }
            MarshalByRefObject obj2 = assembly.CreateInstance(typeName, false, bindingFlags, null, constructorParams, null, null) as MarshalByRefObject;
            if (obj2 == null)
            {
                throw new ArgumentException("typeName must specify a Type that derives from MarshalByRefObject");
            }
            return obj2;
        }

        public string[] GetAssemblies()
        {
            ArrayList list = new ArrayList();
            foreach (Assembly assembly in this.assemblyList)
            {
                list.Add(assembly.FullName);
            }
            return (string[])list.ToArray(typeof(string));
        }

        public object GetStaticPropertyValue(string typeName, string propertyName)
        {
            Type typeByName = this.GetTypeByName(typeName);
            if (typeByName == null)
            {
                throw new ArgumentException("Cannot find a type of name " + typeName + " within the plugins or the common library.");
            }
            return typeByName.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
        }

        public string[] GetSubclasses(string baseClass)
        {
            Type c = Type.GetType(baseClass);
            if (c == null)
            {
                c = this.GetTypeByName(baseClass);
            }
            if (c == null)
            {
                throw new ArgumentException("Cannot find a type of name " + baseClass + " within the plugins or the common library.");
            }
            ArrayList list = new ArrayList();
            foreach (Type type2 in this.typeList)
            {
                if (type2.IsSubclassOf(c) || (type2.GetInterface(c.Name, true) != null))
                {
                    list.Add(type2.AssemblyQualifiedName);
                }
            }
            return (string[])list.ToArray(typeof(string));
        }

        private Type GetTypeByName(string typeName)
        {
            foreach (Type type in this.typeList)
            {
                if (type.FullName == typeName)
                {
                    return type;
                }
            }
            return null;
        }

        public string[] GetTypes()
        {
            ArrayList list = new ArrayList();
            foreach (Type type in this.typeList)
            {
                list.Add(type.FullName);
            }
            return (string[])list.ToArray(typeof(string));
        }

        public void LoadAssembly(string fullname)
        {
            Path.GetDirectoryName(fullname);
            Assembly assembly = Assembly.Load(Path.GetFileNameWithoutExtension(fullname));
            this.assemblyList.Add(assembly);
            foreach (Type type in assembly.GetTypes())
            {
                this.typeList.Add(type);
            }
        }

        public bool ManagesType(string typeName)
        {
            return (this.GetTypeByName(typeName) != null);
        }
    }
}
