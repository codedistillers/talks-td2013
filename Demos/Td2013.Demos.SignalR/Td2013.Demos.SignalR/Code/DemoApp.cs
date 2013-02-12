using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Artiface.Core;
using Artiface.Core.Bootstrapper;
using Artiface.Core.EmbeddedContainer;
using BundleTransformer.Core.Transformers;
using Microsoft.AspNet.SignalR;
using Microsoft.Web.WebPages.OAuth;
using BundleTransformer.Core.Orderers;
using TinyIoC;

namespace Td2013.Demos.SignalR.Code
{
    public class DemoApp : Artiface.Core.ArtifaceApplication
    {
        public DemoApp()
        {
            SetupContainer(new GetAssemblyFromPatternFilter("Td2013.Demos.SignalR"));
            WithOptions(config =>
            {
                config.UseDefaultAttributeRouting = false;
                config.RegisterAuthProvidersFromConfig = true;
            });
        }
    }


    public class ApplicationRoutes : ISiteStartupConfiguratorForType<RouteCollection>
    {
        public void Configure(RouteCollection current, ArtifaceApplication application)
        {
            current.MapHubs();

            
            current.IgnoreRoute("{resource}.axd/{*pathInfo}");

            current.MapRoute(
                name: "Priorities",
                url: "Priorities/{action}/{name}",
                defaults: new { controller = "Priorities", action = "CreateNew", name=UrlParameter.Optional}
            );
            current.MapRoute(
                name: "Account",
                url: "Account/{action}",
                defaults: new { controller = "Account", action = "Login", name = UrlParameter.Optional }
            );
            current.MapRoute(
                name: "Default",
                url: "",
                defaults: new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            );
        }
    }

    public class StyleBundles : ISiteStartupConfiguratorForType<BundleCollection>
    {
        public void Configure(BundleCollection current, ArtifaceApplication application)
        {
            BundleTable.EnableOptimizations = true;
            RegisterJavascriptBundles(current);
            RegisterCssBundles(current);
        }

        public static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-bootstrap.js",
                        "~/Scripts/angular-bootstrap-prettify.js",
                        "~/Scripts/angular-cookies.js",
                        "~/Scripts/angular-loader.js",
                        "~/Scripts/angular-resource.js",
                        "~/Scripts/angular-sanitize.js",
                        "~/Scripts/angular-scenario.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                    "~/Scripts/jquery.signalR-1.0.0-rc2.js",
                    "~/signalr/hubs",
                    "~/Scripts/toastr.min.js"
                ));
        }

        public static void RegisterCssBundles(BundleCollection bundles)
        {
            var cssTransformer = new CssTransformer();
            var nullOrderer = new NullOrderer();
            bundles.UseCdn = true;
            const string googleFonts = "http://fonts.googleapis.com/css?family=Shadows+Into+Light|Raleway|Open+Sans+Condensed:300,400,600,700|Waiting+For+The+Sunrise";


            var fontBundle = new StyleBundle("~/bundles/fonts", googleFonts);
            bundles.Add(fontBundle);

            var bootstrap = new StyleBundle("~/bundles/css").Include("~/Content/less/_application.less");
            bootstrap.Transforms.Add(cssTransformer);
            bootstrap.Orderer = nullOrderer;
            bundles.Add(bootstrap);

            bundles.Add(new StyleBundle("~/Content/css/jqueryui").IncludeDirectory("~/Content/themes/base", "*.css"));
        }

    }


    public class StartupResolverConfiguration : IFactoryStartupConfigurator
    {
        public void Configure(TinyIoCContainer container)
        {
            GlobalHost.DependencyResolver = new TinyIoCDependencyResolver(container);
        }
    }
    public class TinyIoCDependencyResolver : DefaultDependencyResolver
    {
        private readonly TinyIoCContainer container;

        public TinyIoCDependencyResolver(TinyIoCContainer container)
        {
            this.container = container;
        }

        public override object GetService(Type serviceType)
        {
            return container.CanResolve(serviceType) ? container.Resolve(serviceType) : base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var objects = container.CanResolve(serviceType) ? container.ResolveAll(serviceType) : new object[] { };
            return objects.Concat(base.GetServices(serviceType));
        }
    }
}