import { Injectable } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';


@Injectable()
export class AppNotificationService {
    routerEventSubscription: Subscription;
    constructor(private toastr: ToastrService,
        private router: Router) {
        this.clearMessagesOnNavigation();
    }

    clearMessagesOnNavigation() {
        this.routerEventSubscription = this.router.events
            .subscribe((event) => {
                if (event instanceof NavigationStart) {
                    this.toastr.clear();
                }
            });
    }
    success(content?: any, override?: any): any {
        return this.toastr.success(content, 'Success',
            { timeOut: 2000, extendedTimeOut: 1000 });
    }

    error(content?: any, override?: any): any {
        console.log('Notification Error', content);
        return this.toastr.error(content, 'Error', override);
    }

    info(content?: any, override?: any): any {
        return this.toastr.info(content, 'Info',
            { timeOut: 3000 });
    }

    warn(content?: any, override?: any): any {
        return this.toastr.warning(content, 'Warning', override);
    }
}

