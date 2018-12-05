import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { AllResourcesComponent } from 'src/app/modules/resources/all-resources/all-resources.component';
import { AssignedResourcesComponent } from 'src/app/modules/resources/assigned-resources/assigned-resources.component';
import { AssignMprnComponent } from 'src/app/modules/resources/assign-mprn/assign-mprn.component';
import { AssignedMprnToResourcesComponent } from 'src/app/modules/resources/assigned-mprn-to-resources/assigned-mprn-to-resources.component';
import { AssignResourceMprnComponent } from 'src/app/modules/resources/assign-resource-mprn/assign-resource-mprn.component';
import { ViewAssignedResourcesToMprnComponent } from 'src/app/modules/resources/view-assigned-resources-to-mprn/view-assigned-resources-to-mprn.component';
import { MprnHistoryComponent } from 'src/app/modules/resources/mprn-history/mprn-history.component';
import { AuthGuard } from 'src/app/auth-guard';

const routes: Routes = [
    { path: 'all-resources', component: AllResourcesComponent },
    { path: 'assigned-resources', component: AssignedResourcesComponent },
    { path: 'assign-mprn', component: AssignMprnComponent },
    { path: 'assignedmprn', component: AssignedMprnToResourcesComponent },
    { path: 'assign-mprn-resource', component: AssignResourceMprnComponent },
    { path: 'view-assigned-resource-to-mprn', component: ViewAssignedResourcesToMprnComponent },
    {
        path: 'mprn-history',
        canActivate: [AuthGuard],
        component: MprnHistoryComponent
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]

})
export class ResourcesRoutingModule { }