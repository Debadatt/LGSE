import { Injectable } from '@angular/core';
import { UserACLResponse } from 'src/app/models/ui/acl-response';
import { FeatureNames } from 'src/app/app-common-constants';
import { AppNotificationService } from 'src/app/services/notification/app-notification.service';
import { Router } from '@angular/router';
import { PassdataService } from 'src/app/services/passdata.service';

@Injectable()
export class LocalstorageService {
  userACLResponseMap: Map<string, UserACLResponse>;
  userACLResponseArray: UserACLResponse[] = [];

  constructor(
    private appnotificationservice: AppNotificationService,
    private passdataService: PassdataService,
    private router: Router) {
    this.aclData();
  }
  aclData() {
    this.userACLResponseArray = [];
    this.userACLResponseArray = JSON.parse(localStorage.getItem('acl'));
    if (this.userACLResponseArray) {
      this.userACLResponseMap = new Map(this.userACLResponseArray.map(acl => [acl.featureName, acl] as [string, UserACLResponse]));
    }
  }
  checkBaseModuleACL(featurtext: string): boolean {
    this.aclData();
    if (this.userACLResponseMap) {
      const acl = this.userACLResponseMap.get(featurtext);
      if (acl) {
        if (acl.readPermission || acl.updatePermission || acl.createPermission) {
          return true;
        }
      } else {
        return false;
      }
    }
  }// end of fucntion
  checkCreatePermission(featurtext) {
    if (this.userACLResponseMap) {
      const acl = this.userACLResponseMap.get(featurtext);
      if (acl) {
        if (acl.createPermission) {
          return true;
        }
      } else {
        return false;
      }
    }
  }// end of block
  // code block for cheking update perminission.
  checkUpdatePermission(featurtext) {
    if (this.userACLResponseMap) {
      const acl = this.userACLResponseMap.get(featurtext);
      if (acl) {
        if (acl.updatePermission) {
          return true;
        }
      } else {
        return false;
      }
    }
  }// end of block
  // code block for cheking read perminission.
  checkReadPermission(featurtext) {
    if (this.userACLResponseMap) {
      const acl = this.userACLResponseMap.get(featurtext);
      if (acl) {
        if (acl.readPermission) {
          return true;
        }
      } else {
        return false;
      }
    }
  }// end of block

  // code block for checking default home page after login
  defaulHomePage() {
    this.aclData();
    if (this.userACLResponseMap) {
      const acl = this.userACLResponseMap.get(FeatureNames.DASHBOARD);
      if (acl) {
        if (acl.readPermission || acl.updatePermission || acl.createPermission) {
          this.router.navigate(['/dashboard']);
        } else if (this.checkBaseModuleACL(FeatureNames.INCIDENT_MANAGEMENT)) {
          this.router.navigate(['/incident']);
        } else if (this.checkBaseModuleACL(FeatureNames.RESOURCE_MANAGEMENT)) {
          this.router.navigate(['/resources/all-resources']);
        } else if (this.checkBaseModuleACL(FeatureNames.PORTAL_MANAGEMENT)) {
          this.router.navigate(['/portalManagment/userList']);
        } else if (this.checkBaseModuleACL(FeatureNames.ASSIGNED_MPRN)) {
          this.router.navigate(['/incident/showProperties']);
        } else {
          this.appnotificationservice.error(" You Dont Have  Access");
        }
      }
    }
  }// end of fucntion
  checkCommonModulePerminission(feature1, feature2) {
    this.aclData();
    if (this.userACLResponseMap) {
      const acl1 = this.userACLResponseMap.get(feature1);
      const acl2 = this.userACLResponseMap.get(feature2);
      if (acl1 || acl2) {
        if (acl1.readPermission || acl2.readPermission) {
          return true;
        }
      } else {
        return false;
      }
    }
  }

  ngOnDestroy(): void {
    this.userACLResponseArray = [];

  }

}
