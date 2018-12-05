import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppCustomPreloader } from './app-custom-preloader';
import { AuthGuard } from './auth-guard';
import { AuthLayoutComponent } from './layouts/auth-layout/auth-layout.component';
import { AppContainerComponent } from './layouts/app-layout/app-container/app-container.component';
import { FacebookComponent } from './public/facebook/facebook.component';
import { TwitterComponent } from './public/twitter/twitter.component';
import { MapsComponent } from './public/maps/maps.component';
import { NoAccessComponent } from 'src/app/custom/no-access/no-access.component';

const routes: Routes = [
    {
        path: '',
        canActivate: [AuthGuard],
        component: AppContainerComponent,
        children: [
            {
                path: '',
                redirectTo: '/auth/login', pathMatch: 'full'
            },
            // {
            //     path: 'home',
            //     loadChildren: './modules/home/home.module#HomeModule',
            //     data: { preload: true }
            // },
            {
                path: 'incident',
                canActivate: [AuthGuard],
                loadChildren: './modules/incident/incident.module#IncidentModule',
                data: { preload: true }
            },
            {
                path: 'dashboard',
                canActivate: [AuthGuard],
                loadChildren: './modules/dashbord/dashbord-module#DashbordModule',
                data: { preload: true }
            },
            {
                path: 'resources',
                canActivate: [AuthGuard],
                loadChildren: './modules/resources/resources-module#ResourcesModule'
            },                
            {
                path: 'portalManagment',
                canActivate: [AuthGuard],
                loadChildren: './modules/portalManagment/portalManagment.module#PortalManagmentModule',
                data: { preload: false }
            }
            // {
            //     path: 'portalmgmt',
            //     loadChildren: './modules/portalmgmt/portalmgmt.module#PortalMgmtModule',
            //     data: { preload: false }
            // }
        ]
    },
    {
        path: '',
        component: AuthLayoutComponent,
        children: [
            {
                path: 'auth',
                loadChildren: './modules/auth/auth.module#AuthModule',
                data: { preload: true }
            }
            // {
            //     path: 'incident',
            //     loadChildren: './modules/incident/incident.module#IncidentModule',
            //     data: { preload: false }
            // },
        ]
    },
    {
        path: 'facebook',
        component: FacebookComponent
    },
    {
        path: 'twitter',
        component: TwitterComponent
    },
    {
        path: 'map',
        component: MapsComponent
    },
    {
        path: 'noaccess',
        component: NoAccessComponent
    } ,
    {
        path: '**',
        redirectTo: '/auth/login', pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, { preloadingStrategy: AppCustomPreloader, useHash: true })],
    exports: [RouterModule],
    providers: [AppCustomPreloader]
})
export class AppRoutingModule { }
