#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines an static class which contains extension methods of <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified type has parameter less constructor..
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// <c>true</c> if parameter less constructor exists; otherwise, <c>false</c>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type instance)
        {
            Invariant.IsNotNull(instance, "instance");

            return instance.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Any(ctor => ctor.GetParameters().Length == 0);
        }

        /// <summary>
        /// Gets the public types of the given assembly.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> PublicTypes(this Assembly instance)
        {
            IEnumerable<Type> types = null;

            if (instance != null)
            {
                try
                {
                    types = instance.GetTypes().Where(type => (type != null) && type.IsPublic && type.IsVisible).ToList();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }
            }

            return types ?? Enumerable.Empty<Type>();
        }

        /// <summary>
        /// Gets the public types of the given assemblies.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> PublicTypes(this IEnumerable<Assembly> instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.SelectMany(assembly => assembly.PublicTypes());
        }

        /// <summary>
        /// Gets the concretes types of the given assembly.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> ConcreteTypes(this Assembly instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.PublicTypes()
                           .Where(type => (type != null) && type.IsClass && !type.IsAbstract && !type.IsInterface && !type.IsGenericType).ToList();
        }

        /// <summary>
        /// Gets the concretes types of the given assemblies.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<Type> ConcreteTypes(this IEnumerable<Assembly> instance)
        {
            return (instance == null) ?
                   Enumerable.Empty<Type>() :
                   instance.SelectMany(assembly => assembly.ConcreteTypes());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static IList<SelectListItem> GetEnumSelectList(this Type enumType)
        {
            var selectList = new List<SelectListItem>();
            var values = Enum.GetValues(enumType);
            Array.Sort(values);
            foreach (var enumItem in values)
            {
                var item = new SelectListItem();
                item.Text = enumItem.GetDisplayName();
                item.Value = ((int)enumItem).ToString();

                selectList.Add(item);
            }
            return selectList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumItem"></param>
        /// <returns></returns>
        public static string GetDisplayName(this object enumItem)
        {
            var displayName = enumItem.ToString();
            var type = enumItem.GetType();
            var member = type.GetMember(displayName);
            if (member.Length == 0)
            {
                return displayName;
            }
            var displayAttributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            if (displayAttributes != null && displayAttributes.Length > 0)
            {
                displayName = displayAttributes[0].GetName();
            }
            else
            {
                var displayNameAttributes = member[0].GetCustomAttributes(typeof(DisplayNameAttribute), false) as DisplayNameAttribute[];
                if (displayNameAttributes != null && displayNameAttributes.Length > 0)
                {
                    displayName = displayNameAttributes[0].DisplayName;
                }
            }

            return displayName;
        }
    }
}