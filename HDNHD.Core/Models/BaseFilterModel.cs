﻿using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HDNHD.Core.Models
{
    public abstract class BaseFilterModel<T> where T : class
    {
        /// <summary>
        ///     apply this to <code>items</code> then return
        /// </summary>
        /// <requires>
        ///     items != null
        /// </requires>
        /// <param name="items">items to be filtered</param>
        public IQueryable<T> ApplyFilter(IQueryable<T> items)
        {
            ApplyFilter(ref items);

            return items;
        }

        /// <summary>
        ///     apply this to modify <code>items</code>
        /// </summary>
        /// <requires>
        ///     items neq null
        /// </requires>
        /// <modifies>
        ///     items
        /// </modifies>
        /// <param name="items">items to be filtered</param>
        public abstract void ApplyFilter(ref IQueryable<T> items);

        #region utils
        /// <summary>
        /// returns the filtering mode
        /// NOTE: 
        ///     (1) This can be use to check if user ! applying filter 
        ///         -> set default values for filter criterias
        ///     (2) Mode hidden field is required for each filter form
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// returns the underlying DataContext behind the IQueryable in LINQ to SQL
        /// </summary>
        /// <requires>q neq null</requires>
        public DataContext GetDataContext(IQueryable q)
        {
            if (!q.GetType().FullName.StartsWith("System.Data.Linq.DataQuery`1")) return null;
            var field = q.GetType().GetField("context", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) return null;
            return field.GetValue(q) as DataContext;
        }

        #endregion
    }
}