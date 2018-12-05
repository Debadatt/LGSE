import { Component, OnInit } from '@angular/core';
import { PassdataService } from '../../../services/passdata.service';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { Router } from '@angular/router';
import { Userroles } from '../../../modules/portalManagment/roleManagment/roleManagement.component';
import { UserRoleSelected } from '../../../models/ui/user.role.selected';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  feature = FeatureNames;
  userRoleName:string;
  constructor(public localstorageservice: LocalstorageService,
    private router: Router,
public passdataService: PassdataService) { }

  ngOnInit() {
    let localStorageUserRole = localStorage.getItem('userRole');
    let localStoreUserRole : UserRoleSelected = JSON.parse(localStorageUserRole);
    let selecteUserRole = new UserRoleSelected();
    selecteUserRole.roleName = this.userRoleName || localStoreUserRole.roleName;
    this.userRoleName=selecteUserRole.roleName;
    console.log("RoleName:::",selecteUserRole.roleName);
  }
  navAssignedMprn()
  {
    this.passdataService.assignedMPRN=true;
    this.router.navigate(['/incident/showProperties']);
  }

}
