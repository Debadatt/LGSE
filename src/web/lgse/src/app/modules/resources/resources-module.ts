import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppCommonModule } from '../../app-common.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { AllResourcesComponent } from 'src/app/modules/resources/all-resources/all-resources.component';
import { ResourcesRoutingModule } from 'src/app/modules/resources/resources-routing.module';
import { ResourcesServiceService } from 'src/app/services/resources/resources-service.service';
import { AssignedResourcesComponent } from 'src/app/modules/resources/assigned-resources/assigned-resources.component';
import { AssignMprnComponent } from 'src/app/modules/resources/assign-mprn/assign-mprn.component';
import { AssignedMprnToResourcesComponent } from 'src/app/modules/resources/assigned-mprn-to-resources/assigned-mprn-to-resources.component';
import { AssignResourceMprnComponent } from 'src/app/modules/resources/assign-resource-mprn/assign-resource-mprn.component';
import { ViewAssignedResourcesToMprnComponent } from 'src/app/modules/resources/view-assigned-resources-to-mprn/view-assigned-resources-to-mprn.component';
import { MprnHistoryComponent } from 'src/app/modules/resources/mprn-history/mprn-history.component';



@NgModule({
    declarations: [AllResourcesComponent,
        AssignMprnComponent,
        AssignedMprnToResourcesComponent,
        AssignResourceMprnComponent ,
        ViewAssignedResourcesToMprnComponent,
        MprnHistoryComponent,
        AssignedResourcesComponent],
    imports: [CommonModule,
        AppCommonModule,
        ResourcesRoutingModule,
        CommonModule,
        ReactiveFormsModule,
        TranslateModule,
        FormsModule],
    exports: [],
    providers: [
        ResourcesServiceService],
})
export class ResourcesModule { }