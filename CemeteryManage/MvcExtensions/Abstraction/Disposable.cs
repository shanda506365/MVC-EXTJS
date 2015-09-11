#region Copyright
// Copyright (c) 2009 - 2010, Kazi Manzur Rashid <kazimanzurrashid@gmail.com>.
// This source is subject to the Microsoft Public License. 
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL. 
// All other rights reserved.
#endregion

namespace MvcExtensions
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Defines a base class which holds reference of one or more resource object.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Disposable"/> is reclaimed by garbage collection.
        /// </summary>
        [DebuggerStepThrough]
        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources.
        /// </summary>
        [DebuggerStepThrough]
        protected virtual void DisposeCore()
        {
        }

        [DebuggerStepThrough]
        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                DisposeCore();
            }

            disposed = true;
        }
    }
}