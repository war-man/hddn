﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDNHD.Core.Models
{
    public class Pager
    {
        public const int SHOW_ALL = 0;
        private int page;
        private int pageSize;
        private int noItems;

        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                if (value > 0)
                {
                    page = value;
                }
            }
        }

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value >= 0)
                {
                    pageSize = value;
                }
            }
        }

        public int NoItems
        {
            get
            {
                return noItems;
            }
            set
            {
                if (value >= 0)
                {
                    noItems = value;
                }
            }
        }

        public int NoPages
        {
            get
            {
                return pageSize != SHOW_ALL ? (int)Math.Ceiling((double)noItems / pageSize) : 1;
            }
        }

        public Pager()
        {
            page = 1;
            pageSize = 25;
        }

        public HtmlString UrlFirst()
        {
            return UrlPage(1);
        }

        public HtmlString UrlPrev()
        {
            return UrlPage(Page == 1 ? 1 : Page - 1);
        }

        public HtmlString UrlNext()
        {
            return UrlPage(Page == NoPages ? NoPages : Page + 1);
        }

        public HtmlString UrlLast()
        {
            return UrlPage(NoPages);
        }

        public HtmlString UrlPage(int page)
        {
            return BuildQueryStringUrl(HttpContext.Current.Request.Url.ToString(), new string[] { "Page=" + page });
        }

        public HtmlString UrlPageSize(int pageSize)
        {
            return BuildQueryStringUrl(HttpContext.Current.Request.Url.ToString(), new string[] { "PageSize=" + pageSize });
        }

        /// <summary>
        /// Get URL With QueryString Dynamically
        /// </summary>
        /// <param name="url">URI With/Without QueryString</param>
        /// <param name="newQueryStringArr">New QueryString To Append</param>
        /// <returns>Return Url + Existing QueryString + New/Modified QueryString</returns>
        public static HtmlString BuildQueryStringUrl(string url, string[] newQueryStringArr)
        {
            string plainUrl;
            var queryString = string.Empty;

            var newQueryString = string.Join("&", newQueryStringArr);

            if (url.Contains("?"))
            {
                var index = url.IndexOf('?');
                plainUrl = url.Substring(0, index); //URL With No QueryString
                queryString = url.Substring(index + 1);
            }
            else
            {
                plainUrl = url;
            }

            var nvc = HttpUtility.ParseQueryString(queryString);
            var qscoll = HttpUtility.ParseQueryString(newQueryString);

            var queryData = string.Join("&",
                nvc.AllKeys.Where(key => !qscoll.AllKeys.Any(newKey => newKey.Contains(key))).
                    Select(key => string.Format("{0}={1}",
                        HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(nvc[key]))).ToArray());
            //Fetch Existing QueryString Except New QueryString

            var delimiter = nvc.HasKeys() && !string.IsNullOrEmpty(queryData) ? "&" : string.Empty;
            var queryStringToAppend = "?" + newQueryString + delimiter + queryData;

            return new HtmlString(plainUrl + queryStringToAppend);
        }

        #region actions
        /// <summary>
        ///     apply this to modify <tt>items</tt> 
        /// </summary>
        /// <requires>
        ///     items != null
        /// </requires>
        /// <modifies>
        ///     this.NoItems /\ items
        /// </modifies>
        /// <param name="items">items to be filtered</param>
        public void ApplyPager<T>(ref IQueryable<T> items) where T : class
        {
            items = ApplyPager(items);
        }

        /// <summary>
        ///     apply this to <tt>items</tt> then return
        /// </summary>
        /// <requires>
        ///     items != null
        /// </requires>
        /// <modifies>
        ///     this.NoItems
        /// </modifies>
        /// <param name="items">items to be filtered</param>
        public IQueryable<T> ApplyPager<T>(IQueryable<T> items) where T : class
        {
            NoItems = items.Count();

            if (PageSize != Pager.SHOW_ALL)
            {
                items = items.Skip((Page - 1) * PageSize);
                items = items.Take(PageSize);
            }

            return items;
        }
        #endregion
    }
}