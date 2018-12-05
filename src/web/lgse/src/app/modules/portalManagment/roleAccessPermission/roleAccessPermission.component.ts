import { Component, OnInit, NgModule, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import { RolesService } from '../../../services/roles/roles.service';
import { RoleListResponse, AssigningUsersToRoleRequest, RoleAccessPermissionResponse } from '../../../models/api/portalManagment/role.model';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
    selector: 'app-roleAccessPermission',
    templateUrl: './roleAccessPermission.component.html',
    styleUrls: ['./roleAccessPermission.component.css']
})

export class RoleAccessPermission implements OnInit {
    displayedColumns = ['featureText', 'readPermission', 'createPermission', 'updatePermission'];

    //Variable Declaration
    id: string;
    roleAccessPermissionResponse: RoleAccessPermissionResponse[] = [];
    dataModelAccessPermission: RoleAccessPermissionResponse[] = [];
    dataSource;
    roletListResponse: RoleListResponse[] = [];
    feature = FeatureNames;
    roleName:string;

    constructor(private router: Router,
        private activtedroute: ActivatedRoute,
        private rolesService: RolesService,
        private apiErrorService: ApiErrorService,
        private appNotificationService: AppNotificationService,
        public localstorageservice: LocalstorageService
    ) { }
    ngOnInit() {
        //Getting Role Id and Role Name from RoleList
        this.id = this.activtedroute.snapshot.params.roleId;
        this.roleName=this.activtedroute.snapshot.params.roleName;
        //Get Access Permission for Selected Role
        this.getRoleAccessPermission();
    }
    getRoleAccessPermission() {
        this.rolesService.getRoleAccessPermission(this.id).subscribe(payloadResponse => {
            this.roleAccessPermissionResponse = payloadResponse;
            this.dataSource = new MatTableDataSource<RoleAccessPermissionResponse>(this.roleAccessPermissionResponse);
        });
    }
    checkedReadPermission(featureName) {
        //If Update Or Create Permission set True automatically Read Permission Set to True
        for (let i = 0; i < this.roleAccessPermissionResponse.length; i++) {
            if (this.roleAccessPermissionResponse[i].featureText == featureName) {
                if (this.roleAccessPermissionResponse[i].createPermission == true || this.roleAccessPermissionResponse[i].updatePermission == true) {
                    this.roleAccessPermissionResponse[i].readPermission = true
                }
            }
        }

    }
    submit() {
        var dataModel = new RoleAccessPermissionResponse();
    
        for (let i = 0; i < this.roleAccessPermissionResponse.length; i++) {
            const roleAccessPermissionResponseObject = new RoleAccessPermissionResponse();

            if (this.roleAccessPermissionResponse[i].createPermission == true) {
                roleAccessPermissionResponseObject.createPermission = true;
            }
            else {
                roleAccessPermissionResponseObject.createPermission = false;
            }
            if (this.roleAccessPermissionResponse[i].updatePermission == true) {
                roleAccessPermissionResponseObject.updatePermission = true;
            }
            else {
                roleAccessPermissionResponseObject.updatePermission = false;
            }
            if (this.roleAccessPermissionResponse[i].readPermission == true) {
                roleAccessPermissionResponseObject.readPermission = true;
            }
            else {
                roleAccessPermissionResponseObject.readPermission = false;
            }

            roleAccessPermissionResponseObject.featureId = this.roleAccessPermissionResponse[i].featureId
            roleAccessPermissionResponseObject.roleId = this.id;

            this.dataModelAccessPermission.push(roleAccessPermissionResponseObject);
        }

        // Call API for posting Role Access Permission
        this.rolesService.setRoleAccessPermission(this.dataModelAccessPermission).subscribe(
            response => {
                this.router.navigate(['/portalManagment/roleList']).then(this.appNotificationService.success('Role Assignement To Modules Successfully!'));
            },
            (error) => {
                //Error Handling
                this.apiErrorService.handleError(error, () => {
                    this.appNotificationService.error("Failed to Assignment!");
                });
            });
    }
}

















