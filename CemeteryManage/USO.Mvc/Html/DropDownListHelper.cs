using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using USO.Domain;
using USO.Dto;

namespace USO.Mvc.Html
{
    public static class DropDownListHelper
    {
        private static IList<SelectListItem> ConvertToSelectList<TSourceType, TText, TValue>(this IEnumerable<TSourceType> sourceList,
                                                                                   Expression<Func<TSourceType, TText>> textSelector,
                                                                                   Expression<Func<TSourceType, TValue>> valueSelector)
        {
            var selectList = new List<SelectListItem>();
            var textMemberExpression = (MemberExpression)textSelector.Body;
            var textProperty = (PropertyInfo)textMemberExpression.Member;
            var textGetMethod = textProperty.GetGetMethod();

            var valueMemberExpression = (MemberExpression)valueSelector.Body;
            var valueProperty = (PropertyInfo)valueMemberExpression.Member;
            var valueGetMethod = valueProperty.GetGetMethod();


            foreach (var item in sourceList)
            {
                var text = textGetMethod.Invoke(item, null);
                var tvalue = valueGetMethod.Invoke(item, null);
                selectList.Add(new SelectListItem() { Text = text.ToString(), Value = tvalue.ToString() });
            }
            return selectList;
        }

       

      
    }
}
