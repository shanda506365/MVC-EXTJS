﻿
namespace USO.Utility
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using USO.Domain;

    /// Allows static reflection on members (methods, fields, properties).
    /// This code has been adapted from the following blog post:
    /// http://wekempf.spaces.live.com/blog/cns!D18C3EC06EA971CF!694.entry
    /// Example:
    /// class Program {
    ///    public void Foo() { }
    ///    static void Main(string[] args) {
    ///        Console.WriteLine(ReflectOn&lt;Progra&gt;.GetMethod(p =&gt; p.Foo()).Name);
    ///    }
    /// }
    /// </summary>
    public static class ReflectOn<T>
    {
        public static MemberInfo GetMember(Expression<Action<T>> expression)
        {
            Check.Argument.IsNotNull(expression, "expression");

            return Reflect.GetMemberInfo(expression);
        }

        public static MemberInfo GetMember<TResult>(Expression<Func<T, TResult>> expression)
        {
            Check.Argument.IsNotNull(expression, "expression");

            return Reflect.GetMemberInfo(expression);
        }

        public static MethodInfo GetMethod(Expression<Action<T>> expression)
        {
            MethodInfo method = GetMember(expression) as MethodInfo;
            Check.Argument.IsNotNull(method, "expression");//"Expression is not a method call");

            return method;
        }

        public static PropertyInfo GetProperty<TResult>(Expression<Func<T, TResult>> expression)
        {
            PropertyInfo property = GetMember(expression) as PropertyInfo;
            Check.Argument.IsNotNull(property, "expression");//"Expression is not a property access");

            return property;
        }

        public static FieldInfo GetField<TResult>(Expression<Func<T, TResult>> expression)
        {
            FieldInfo field = GetMember(expression) as FieldInfo;
            Check.Argument.IsNotNull(field, "expression");//"Expression is not a field access");

            return field;
        }

        public static string NameOf(Expression<Action<T>> expression)
        {
            return Reflect.GetNameOf(expression);
        }

        public static string NameOf<TResult>(Expression<Func<T, TResult>> expression)
        {
            return Reflect.GetNameOf(expression);
        }
    }
}