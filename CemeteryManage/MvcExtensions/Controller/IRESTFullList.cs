﻿#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System.Web.Mvc;

    /// <summary>
    /// Defines an interface to list resource in RESTFul way.
    /// </summary>
    public interface IRESTFullList
    {
        /// <summary>
        /// List the resources.
        /// </summary>
        /// <returns></returns>
        ActionResult Index();
    }
}