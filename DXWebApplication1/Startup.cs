using System;
using System.IO;
using DashboardWebApplication1;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Sql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace DXWebApplication1 {
    public class Startup {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
            DashboardExportSettings.CompatibilityMode = DashboardExportCompatibilityMode.Restricted;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDevExpressControls();
            services.AddMvc()
            .AddDefaultReportingControllers()
            .AddDefaultDashboardController((configurator, serviceProvider) => {
                 configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));

                 DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(FileProvider.GetFileInfo("Data/Dashboards").PhysicalPath);
                 configurator.SetDashboardStorage(dashboardFileStorage);

                 DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

                 // Registers an SQL data source.
                 DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("SQL Data Source", "NWindConnectionString");
                 sqlDataSource.DataProcessingMode = DataProcessingMode.Client;
                 SelectQuery query = SelectQueryFluentBuilder
                     .AddTable("Categories")
                     .Join("Products", "CategoryID")
                     .SelectAllColumns()
                     .Build("Products_Categories");
                 sqlDataSource.Queries.Add(query);
                 dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());

                 // Registers an Object data source.
                 DashboardObjectDataSource objDataSource = new DashboardObjectDataSource("Object Data Source");
                 dataSourceStorage.RegisterDataSource("objDataSource", objDataSource.SaveToXml());

                 // Registers an Excel data source.
                 DashboardExcelDataSource excelDataSource = new DashboardExcelDataSource("Excel Data Source");
                 excelDataSource.FileName = FileProvider.GetFileInfo("Data/Sales.xlsx").PhysicalPath;
                 excelDataSource.SourceOptions = new ExcelSourceOptions(new ExcelWorksheetSettings("Sheet1"));
                 dataSourceStorage.RegisterDataSource("excelDataSource", excelDataSource.SaveToXml());

                 configurator.SetDataSourceStorage(dataSourceStorage);

                 configurator.DataLoading += (s, e) => {
                     if (e.DataSourceName == "Object Data Source")
                     {
                         e.Data = Invoices.CreateData();
                     }
                 };
             });
            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureReportDesigner(designerConfigurator => {
                    designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                });
            });
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            var reportDirectory = Path.Combine(env.ContentRootPath, "Reports");
            DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new DevExpressReportStorage(reportDirectory));
            DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseDevExpressControls();
            app.UseMvc(routes => {
                routes.MapDashboardRoute("api/dashboard");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            
            app.UseSpa(spa => {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}