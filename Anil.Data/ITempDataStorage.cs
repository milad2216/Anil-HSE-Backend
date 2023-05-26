﻿using System;
using System.Linq;

namespace Anil.Data
{
    /// <summary>
    /// Represents temporary storage
    /// </summary>
    /// <typeparam name="T">Storage record mapping class</typeparam>
    public interface ITempDataStorage<T> : IQueryable<T>, IDisposable, IAsyncDisposable where T : class
    {
    }
}