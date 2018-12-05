import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LoginStatusProviderService } from './services/login-status-provider.service';
import { Breadcrumbs } from 'src/app/models/breadcrumbs-struct';
import { FeatureNames, HistoryUrl, AssignedMprn } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';



@Injectable()
export class AuthGuard implements CanActivate {
    breadcrumbs: Breadcrumbs[] = [];
    selectedmodule = '';
    breadcrumbsmap: Map<string, string>;
    constructor(private router: Router,
        private loginStatusProviderService: LoginStatusProviderService,
        private localstorageService: LocalstorageService
    ) {
        this.createMAP();
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        console.log('can activate this.state.url', state.url);
        if (localStorage.getItem('accessToken')) {
            let newurl = state.url.replace(/\//g, ' ');
            let routelist = newurl.split(' ');
            let historyurl = state.url.split(';');
            console.log('history url');
            console.log(historyurl);
            console.log(routelist);
            if (routelist.length > 0 && routelist) {
                // if route is came for common module features so check mannually their perminissions. 
                if (historyurl && historyurl.length > 0) {
                    switch (historyurl[0]) {
                        case HistoryUrl:
                        //cheking for history feature
                            if (this.localstorageService.checkCommonModulePerminission(FeatureNames.INCIDENT_MANAGEMENT, FeatureNames.RESOURCE_MANAGEMENT)) {
                                return true;
                            } else {
                                this.router.navigate(['/noaccess']);
                                return false;
                            }
                        case AssignedMprn:
                          // cheking for assigned mprn feature.
                            if (this.localstorageService.checkCommonModulePerminission(FeatureNames.ASSIGNED_MPRN, FeatureNames.INCIDENT_MANAGEMENT)) {
                                return true;
                            } else {
                                this.router.navigate(['/noaccess']);
                                return false;
                            }
                        default:
                        // if path is not mattched then break it.
                            break;
                    }
                }
                // end of code

                this.selectedmodule = this.breadcrumbsmap.get(routelist[1]);
                if (this.localstorageService.checkBaseModuleACL(this.selectedmodule)) {
                    return true;
                } else {
                    this.router.navigate(['/noaccess']);
                    return false;

                }
            }
            return true;
        }
        this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
    createMAP() {
        this.breadcrumbs.push({ key: 'dashboard', value: FeatureNames.DASHBOARD });
        this.breadcrumbs.push({ key: 'resources', value: FeatureNames.RESOURCE_MANAGEMENT });
        this.breadcrumbs.push({ key: 'incident', value: FeatureNames.INCIDENT_MANAGEMENT });
        this.breadcrumbs.push({ key: 'portalManagment', value: FeatureNames.PORTAL_MANAGEMENT });
        //  this.breadcrumbs.push({ key: 'ASSIGNEDMPRN', value: FeatureNames.ASSIGNED_MPRN });
        this.breadcrumbsmap = new Map(this.breadcrumbs.map(x => [x.key, x.value] as [string, string]));
    }
}
