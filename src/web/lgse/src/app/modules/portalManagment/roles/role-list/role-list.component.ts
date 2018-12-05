import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { RolesService } from '../../../../services/roles/roles.service';
import { RoleListResponse,PassRoleData } from '../../../../models/api/portalManagment/role.model';
import { PassdataService } from '../../../../services/passdata.service';
import { CustomSearchData, CustomSearchDataMain } from 'src/app/models/search/custom-search-data';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';


@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})

export class RoleListComponent implements OnInit {
  displayedColumns = ['roleName', 'description', 'action'];

  //Variable Declaration
  customSearchDataMain = new CustomSearchDataMain();
  selectedresourcetype: string;
  customSearchData: CustomSearchData[] = [];
  selected: string[] = [];
  dataSource;
  isvisible = false;
  isExpanded = false;
  count = 0;
  feature = FeatureNames;

  roletListResponse: RoleListResponse[] = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private router: Router,
    private roleService: RolesService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    private passdataservice: PassdataService,
    public localstorageservice: LocalstorageService
  ) { }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  ngOnInit() {
    this.getRoleList();
  }

  //Get All Roles
  getRoleList() {
    this.roleService.getRoleDataList().subscribe(payloadResponse => {
      this.roletListResponse = payloadResponse;
      this.dataSource = new MatTableDataSource<RoleListResponse>(this.roletListResponse);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    });
  }

  //Edit Role
  editRole(id): void {
    this.router.navigate(['/portalManagment/editRole', id]);
  }

  //Assign Role to Mutiple users
  assignRole(id,roleName): void {
    const roleData = new PassRoleData();
    roleData.roleId = id;
    roleData.roleName = roleName;
    console.log("roleData",roleData);
    this.router.navigate(['/portalManagment/assignRole', roleData]);
  }

  //Add New Role
  addRole(): void {
    this.router.navigate(['/portalManagment/addRole']);
  }
  //Access Permission Roles -->Modules
  accessPermission(id,roleName):void{
    const roleData = new PassRoleData();
    roleData.roleId = id;
    roleData.roleName = roleName;
    console.log("roleData",roleData);
    this.router.navigate(['/portalManagment/roleAccessPermission', roleData]);
  }

  // code block for Refresh Button
 refresh(): void {
  this.getRoleList();
}
}














