using Enrollment.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Enrollment
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static string connStr;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //using the connection string from web.config which is linked
            connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //The CreateServices() function is defiend outside the main function in this file
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }

        }


        //FluentMigrator codes
        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection() // this function is why we need 'Microsoft.Extensions.DependencyInjection;'
                                           // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add PostgreSQL support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connStr)
                    // Define the assembly containing the migration
                    .ScanIn(typeof(_001_Account).Assembly,typeof(_002_Student).Assembly).For.Migrations())
                    // Enable logging to console in the FluentMigrator way
                    .AddLogging(lb => lb.AddFluentMigratorConsole())
                    // Build the service provider
                    .BuildServiceProvider(false);
        }


        /// <summary>
        /// Update the database
        /// </sumamry>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

    }
}
