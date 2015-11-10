﻿using System.Web;
using System.Web.Optimization;

namespace Points.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/PointsMVCApp")
                .IncludeDirectory("~/Scripts/Controllers", "*.js")
                .IncludeDirectory("~/Scripts/Factories", "*.js")
                .Include("~/Scripts/PointsMVCApp.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
