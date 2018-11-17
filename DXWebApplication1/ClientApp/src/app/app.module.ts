import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav.menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { DxReportViewerModule, DxReportDesignerModule } from 'devexpress-reporting-angular';
import { ReportViewerComponent } from './reportviewer/report-viewer';
import { ReportDesignerComponent } from './reportdesigner/report-designer';
import { DashboardDesignerComponent } from './dashboard-designer/dashboard-designer.component';
import { DxPivotGridModule, DxChartModule } from 'devextreme-angular';
import { PivotGridComponent } from './pivot-grid/pivot-grid.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        ReportViewerComponent,
        ReportDesignerComponent,
        DashboardDesignerComponent,
        PivotGridComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        DxReportViewerModule,
        DxReportDesignerModule,
        DxPivotGridModule,
        DxChartModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'designer', component: ReportDesignerComponent },
            { path: 'viewer', component: ReportViewerComponent },
            { path: 'dashboard-designer', component: DashboardDesignerComponent },
            { path: 'pivot-grid', component: PivotGridComponent }
        ])
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }