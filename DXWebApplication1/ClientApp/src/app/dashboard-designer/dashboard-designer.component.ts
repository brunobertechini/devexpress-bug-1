import { Component, OnInit, AfterViewInit, ElementRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { DashboardControl, ResourceManager } from 'devexpress-dashboard';

@Component({
    selector: 'app-dashboard-designer',
    encapsulation: ViewEncapsulation.None,
  templateUrl: './dashboard-designer.component.html',
    styleUrls: [
        "../../../node_modules/jquery-ui/themes/base/all.css",
        "../../../node_modules/devextreme/dist/css/dx.common.css",
        "../../../node_modules/devextreme/dist/css/dx.light.css",
        "../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.common.css",
        "../../../node_modules/@devexpress/analytics-core/dist/css/dx-analytics.light.css",
        "../../../node_modules/@devexpress/analytics-core/dist/css/dx-querybuilder.css",
        "../../../node_modules/devexpress-dashboard/dist/css/dx-dashboard.light.css"
    ]
})
export class DashboardDesignerComponent implements OnInit, AfterViewInit {

    @ViewChild('dashboardDesigner') _dashboardDesigner: ElementRef;
    public dashboardControl: any;

  constructor() { }

  ngOnInit() {
  }

    ngAfterViewInit(): void {
        // Adds required HTML resources to the DOM.
        ResourceManager.embedBundledResources();

        // Creates a new Web Dashboard control with specified settings.
        this.dashboardControl = new DashboardControl(this._dashboardDesigner.nativeElement, {

            // Configures an URL where the Web Dashboard's server-side is hosted.
            endpoint: 'http://localhost:61718/api/dashboard',
            workingMode: 'Designer', // values: "Viewer" | "Designer" | "ViewerOnly"
            encodeHtml: true,
            initialDashboardId: 'dashboard1',
            // height: '100%',
            initialDashboardState: null,
            loadDefaultDashboard: true,
            onDashboardBeginUpdate: function (args) { },
            onDashboardEndUpdate: function (args) { },
            onItemBeginUpdate: function (args) { },
            onItemEndUpdate: function (args) { },
            showConfirmationOnBrowserClosing: true,
            extensions: {
                'url-state': {
                    includeDashboardIdToUrl: false,
                    includeDashboardStateToUrl: false,
                },
                // 'dashboard-export': {
                //     allowExportDashboard: true,
                //     allowExportDashboardItems: false
                //     // pdfExportOptions: {},
                //     // imageExportOptions: {},
                //     // excelExportOptions: {}
                // },
                'viewer-api': {
                    onItemClick: function (args) { },
                    onItemHover: function (args) { },
                    onItemSelectionChanged: function (args) { },
                    onItemWidgetCreated: function (args) { },
                    onItemWidgetUpdating: function (args) { },
                    onItemWidgetUpdated: function (args) { },
                    onItemElementCustomColor: function (args) { },
                    onItemVisualInteractivity: function (args) { },
                    onItemMasterFilterStateChanged: function (args) { },
                    onItemDrillDownStateChanged: function (args) { },
                    onItemDrillUpStateChanged: function (args) { },
                    onItemActionAvailabilityChanged: function (args) { }
                },
                'dashboard-parameter-dialog': {
                    onDynamicLookUpValuesLoaded: function (args) { }
                },
                'data-source-wizard': {
                    enableCustomSql: true
                }
            }
        });

        this.dashboardControl.render();
    }

}
