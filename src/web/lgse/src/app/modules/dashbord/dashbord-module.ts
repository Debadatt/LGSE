import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../../app-common.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { DashbordService } from 'src/app/services/dashbord/dashbord.service';
import { DashbordContainerComponent } from 'src/app/modules/dashbord/dashbord-container/dashbord-container.component';
import { DashbordRoutingModule } from 'src/app/modules/dashbord/dashbord-routing.module';
import { MprnStatusCountComponent } from 'src/app/modules/dashbord/mprn-status-count/mprn-status-count.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ZonewiseIncidentStatusComponent } from 'src/app/modules/dashbord/zonewise-incident-status/zonewise-incident-status.component';
import { CellwiseIncidentStatusComponent } from 'src/app/modules/dashbord/cellwise-incident-status/cellwise-incident-status.component';
import { UserWiseIncidentSummaryComponent } from 'src/app/modules/dashbord/user-wise-incident-summary/user-wise-incident-summary.component';
import { IncidentLevelAverageTimeTakenComponent } from 'src/app/modules/dashbord/incident-level-average-time-taken/incident-level-average-time-taken.component';
import { ZoneLevelAverageTimeTakenComponent } from 'src/app/modules/dashbord/zone-level-average-time-taken/zone-level-average-time-takan.component';
import { CellLevelAverageTimeTakenComponent } from 'src/app/modules/dashbord/cell-level-average-time-taken/cell-level-average-time-taken.component';
import { UserLevelAverageTimeTakenComponent } from 'src/app/modules/dashbord/user-level-average-time-taken/user-level-average-time-taken.component';
import { EngineeringCapacityReportComponent } from 'src/app/modules/dashbord/engineering-capacity-report/engineering-capacity-report.component';
@NgModule({
    declarations: [
        DashbordContainerComponent,
        CellwiseIncidentStatusComponent,
        UserLevelAverageTimeTakenComponent,
        EngineeringCapacityReportComponent,
        IncidentLevelAverageTimeTakenComponent,
        CellLevelAverageTimeTakenComponent,
        ZonewiseIncidentStatusComponent,
        ZoneLevelAverageTimeTakenComponent,
        MprnStatusCountComponent,
        UserWiseIncidentSummaryComponent
    ],
    imports: [
        AppCommonModule,
        CommonModule,
        NgxChartsModule,
        ReactiveFormsModule,
        TranslateModule,
        DashbordRoutingModule,
        FormsModule
    ],
    exports: [],
    providers: [
        DashbordService
    ],
})
export class DashbordModule {

}