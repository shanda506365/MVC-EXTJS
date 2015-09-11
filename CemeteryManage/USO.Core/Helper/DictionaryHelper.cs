using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace USO.Core.Helper
{
    public class DictionaryHelper<T> where T : class ,new ()
    {
        /// <summary>
        /// 设置字典属性值
        /// </summary>
        /// <param name="fieldName">需要赋值的字段名称</param>
        /// <param name="value">对应值</param>
        public static void SetValue(T t, string fieldName, object value)
        {
            PropertyInfo[] pInfos = t.GetType().GetProperties();
            foreach (PropertyInfo p in pInfos)
            {
                if (p.Name.Equals(fieldName))
                {
                    p.SetValue(t, value);
                    break;
                }
            }
        }

        /// <summary>
        /// Excel与数据库字段名称翻译
        /// </summary>
        /// <typeparam name="U">用于字典翻译的类型</typeparam>
        /// <param name="list">需要翻译的数据</param>
        /// <param name="u">翻译实体</param>
        /// <returns></returns>
        public static DataTable TranslationExport<U>(List<T> list,U u)
        {
            DataTable dt=new DataTable();
            PropertyInfo[] pInfos = u.GetType().GetProperties();

            foreach (T t in list)
            {
                PropertyInfo[] tInfos = t.GetType().GetProperties();

                foreach (PropertyInfo tInfo in tInfos)
                {
                    DataRow row=dt.NewRow();
                    foreach (PropertyInfo pInfo in pInfos)
                    {
                        object obj = pInfo.GetCustomAttribute(typeof(DescriptionAttribute), false);
                        if (obj != null)
                        {
                            string fieldName = ((DescriptionAttribute)obj).Description;
                            if (tInfo.Name.Equals(pInfo.Name))
                            {
                                DataColumn column = new DataColumn(fieldName);
                                column.DefaultValue = tInfo.GetValue(tInfo);
                                dt.Columns.Add(column);
                            }
                        }
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        /// <summary>
        /// 导入Excel做字段名称对应翻译
        /// </summary>
        /// <typeparam name="U">字典实体类</typeparam>
        /// <param name="dt">导入的数据</param>
        /// <param name="u">字典实体类</param>
        public static List<T> TranslationImport<U>(DataTable dt,U u)
        {
            List<T> list = new List<T>();

            PropertyInfo[] pInfos = u.GetType().GetProperties();

            foreach(DataRow row in dt.Rows)
            {
                T t = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    foreach (PropertyInfo pInfo in pInfos)
                    {
                        object obj = pInfo.GetCustomAttribute(typeof(DescriptionAttribute), false);
                        if (obj != null)
                        {
                            string fieldName = ((DescriptionAttribute)obj).Description;
                            if (column.ColumnName.Equals(fieldName))
                            {
                                string value=row[column].ToString();
                                if (pInfo.PropertyType.FullName.Equals("System.Decimal"))
                                {
                                    decimal capital = 0;
                                    decimal.TryParse(value, out capital);
                                    t.GetType().GetProperty(pInfo.Name).SetValue(t, capital);
                                }
                                else
                                {
                                    if (!System.DBNull.Value.Equals(value))
                                    {
                                        t.GetType().GetProperty(pInfo.Name).SetValue(t, value);
                                    }
                                }
                                
                            }
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }


        /// <summary>
        /// 将字典实体里面的值赋值给对应表实体
        /// </summary>
        /// <typeparam name="U">字典类</typeparam>
        /// <param name="u">字典类实体</param>
        /// <returns>对应表实体</returns>
        public static T DictionaryConvertToModel<U>(U u)
        {
            T t = new T();

            PropertyInfo[] uInfos = u.GetType().GetProperties();
            PropertyInfo[] tInfos = t.GetType().GetProperties();

            foreach (PropertyInfo tInfo in tInfos)
            {
                foreach (PropertyInfo uInfo in uInfos)
                {
                    if (tInfo.Name.Equals(uInfo.Name))
                    {
                        tInfo.SetValue(uInfo.GetValue(uInfo), tInfo);
                    }
                }
            }

            return t;
        }
    }
}
