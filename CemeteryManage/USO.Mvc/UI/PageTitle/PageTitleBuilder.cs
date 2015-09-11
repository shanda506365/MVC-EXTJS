﻿
namespace USO.UI.PageTitle
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using MvcExtensions;

    public class PageTitleBuilder : IPageTitleBuilder
    {
        private readonly List<string> _titleParts;
        private readonly string _titleSeparator;

       

        public void AddTitleParts(params string[] titleParts)
        {
            if (titleParts != null)
                foreach (string titlePart in titleParts)
                    if (!string.IsNullOrEmpty(titlePart))
                        _titleParts.Add(titlePart);
        }

        public void AppendTitleParts(params string[] titleParts)
        {
            if (titleParts != null)
                foreach (string titlePart in titleParts)
                    if (!string.IsNullOrEmpty(titlePart))
                        _titleParts.Insert(0, titlePart);
        }

        public string GenerateTitle()
        {
            return string.Join(_titleSeparator, _titleParts.AsEnumerable().Reverse().ToArray());
        }
    }
}