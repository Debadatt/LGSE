import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RolesService } from '../../../../services/roles/roles.service';
import { RoleAddRequest } from '../../../../models/api/portalManagment/role.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-role-addEdit',
  templateUrl: './role-addEdit.component.html',
  styleUrls: ['./role-addEdit.component.css']
})

export class RoleAddEditComponent implements OnInit {

  //Variable Declaration
  roleAddForm: FormGroup;
  feature = FeatureNames;

  constructor(private router: Router,
    private rolesService: RolesService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.roleAddForm = new FormGroup({
      'roleName': new FormControl(null, [Validators.required]),
      'description': new FormControl(null),
    });
  }

  onSubmit() {
    var dataModel = new RoleAddRequest();

    //Added data into data model
    dataModel.roleName = this.roleAddForm.value.roleName;
    dataModel.description = this.roleAddForm.value.description;

    //Call API for Role Adding Data
    this.rolesService.addRole(dataModel).subscribe(
      response => {
        this.router.navigate(['/portalManagment/roleList']).then(this.appNotificationService.success('Role Created Successfully!'));
      },
      (error) => {
        this.apiErrorService.handleError(error, () => {
          if (error.error.message == "ROLE_EXISTS_ALREADY") {
            this.appNotificationService.error("Role Already Exist!");
          }
        });
      });
  }
}























