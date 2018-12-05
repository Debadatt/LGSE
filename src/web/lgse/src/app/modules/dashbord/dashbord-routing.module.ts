import { NgModule } from '@angular/core';
// import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { DashbordContainerComponent } from 'src/app/modules/dashbord/dashbord-container/dashbord-container.component';
const routes: Routes = [
     { path: '', component: DashbordContainerComponent },
     { path: 'values', component: DashbordContainerComponent }
]

// const routes: Routes = [
//     {
//       path: '',
//       children: [
//         {
//           path: '',
//           component: DashbordContainerComponent
//         },
//         {
//           path: 'dashboard',
//           component: DashbordContainerComponent
//         }        
//       ]
//     }
//   ];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]

})
export class DashbordRoutingModule { }