﻿using StockAnalyzer.WebApi.Utilities;
using StockAnalyzer.Common.Helpers;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace StockAnalyzer.WebApi
{
    /// <summary>
    /// The main class <c>WebApiApplication</c>.
    /// Represents the application execution starting point.
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Sets up the details to start application
        /// </summary>
        protected void Application_Start()
        {
            Environment.SetEnvironmentVariable("BASEDIR", PathHelper.GetRootedPath(), EnvironmentVariableTarget.Process);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.Flush();
            }
            WebUtils.SetUserLocale();
        }
    }
}
