#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a interface which is used to hold the list of the task that would be executed when bootstrapping the application.
    /// </summary>
    public interface IBootstrapperTasksRegistry : IFluentSyntax
    {
        /// <summary>
        /// Gets or sets the task configurations.
        /// </summary>
        /// <value>The tasks.</value>
        IEnumerable<KeyValuePair<Type, Action<object>>> TaskConfigurations { get; }

        /// <summary>
        /// Includes the specified task.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <returns></returns>
        IBootstrapperTasksRegistry Include<TTask>() where TTask : BootstrapperTask;

        /// <summary>
        /// Includes the specified task and also allows to configure.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        IBootstrapperTasksRegistry Include<TTask>(Action<TTask> configure) where TTask : BootstrapperTask;

        /// <summary>
        /// Excludes the specified task which might have  implicitly added.
        /// </summary>
        /// <typeparam name="TTask">The type of the task.</typeparam>
        /// <returns></returns>
        IBootstrapperTasksRegistry Exclude<TTask>() where TTask : BootstrapperTask;
    }
}