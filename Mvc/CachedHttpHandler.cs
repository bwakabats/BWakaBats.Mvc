// *****************************************************
//                     MVC EXPANSION
//                  by  Shane Whitehead
//                  bwakabats@gmail.com
// *****************************************************
//      The software is released under the GNU GPL:
//          http://www.gnu.org/licenses/gpl.txt
//
// Feel free to use, modify and distribute this software
// I only ask you to keep this comment intact.
// Please contact me with bugs, ideas, modification etc.
// *****************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace BWakaBats.Mvc
{
    /// <summary>
    /// Defines a synchronous HTTP handler for JS files
    /// </summary>
    public abstract class CachedHttpHandler : IHttpHandler
    {
        private static Dictionary<string, CachedItem> _cache = new Dictionary<string, CachedItem>();

        /// <summary>
        /// Gets a value indicating whether another request can use the JSHttpHandler instance.
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests for a Javascript file
        /// </summary>
        /// <param name="context">An System.Web.HttpContext object that provides references to the intrinsic server objects used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var response = context.Response;

            response.ContentType = ContentType;
            response.Charset = "";

            string uri = request.Url.AbsoluteUri;
            string physicalPath = GetPhysicalPath(context);
            DateTime contentModified;
            var fileInfo = new FileInfo(physicalPath);
            contentModified = fileInfo.LastWriteTime;
            bool tooLarge;
#if DEBUG || TEST
            tooLarge = true;
#else
            tooLarge = fileInfo.Length > 10000000;

            contentModified = contentModified.AddTicks(-(contentModified.Ticks % TimeSpan.TicksPerSecond));

            if (IsContentSame(request.Headers["If-Modified-Since"], contentModified))
            {
                response.StatusCode = 304;
                response.SuppressContent = true;
                return;
            }
            response.Cache.SetLastModified(contentModified);
#endif
            if (tooLarge)
            {
                response.WriteFile(physicalPath);
            }
            else
            {
                if (!_cache.TryGetValue(uri, out CachedItem cachedItem) || cachedItem.DateTimeLastModified != contentModified)
                {
                    lock (_cache)
                    {
                        if (!_cache.TryGetValue(uri, out cachedItem) || cachedItem.DateTimeLastModified != contentModified)
                        {
                            if (cachedItem == null)
                            {
                                cachedItem = new CachedItem();
                                _cache.Add(uri, cachedItem);
                            }
                            cachedItem.DateTimeLastModified = contentModified;
                            try
                            {
                                cachedItem.Content = Process(context, fileInfo, physicalPath);
                            }
                            catch { }
                        }
                    }
                }

                if (cachedItem.Content is byte[] contentBytes)
                {
                    response.BinaryWrite(contentBytes);
                }
                else if (cachedItem.Content is char[] contentChars)
                {
                    response.Write(contentChars, 0, contentChars.Length);
                }
                else
                {
                    response.Write(cachedItem.Content);
                }
            }
        }

        protected virtual string GetPhysicalPath(HttpContext context)
        {
            return context.Request.PhysicalPath;
        }

        protected abstract string ContentType { get; }

        protected virtual object Process(HttpContext context, FileInfo fileInfo, string physicalPath)
        {
            return File.ReadAllBytes(physicalPath);
        }

#if !DEBUG && !TEST
        private static bool IsContentSame(string modifiedSince, DateTime serverFileLastModified)
        {
            if (modifiedSince != null)
            {
                if (DateTime.TryParse(modifiedSince, out DateTime clientFileLastModified))
                {
                    return clientFileLastModified == serverFileLastModified;
                }
            }
            return false;
        }
#endif

        private class CachedItem
        {
            public DateTime DateTimeLastModified { get; set; }
            public object Content { get; set; }
        }
    }
}
