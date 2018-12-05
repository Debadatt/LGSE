import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { RolesService } from '../../../../services/roles/roles.service';
;
import { RoleEditRequest, } from '../../../../models/api/portalManagment/role.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-role-edit',
  templateUrl: './role-edit.component.html',
  styleUrls: ['./role-edit.component.css']
})

export class RoleEditComponent implements OnInit {

  //variable Declaration
  roleAddForm: FormGroup;
  id: string;
  feature = FeatureNames;

  constructor(private router: Router,
    private rolesService: RolesService,
    private apiErrorService: ApiErrorService,
    private activtedroute: ActivatedRoute,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.roleAddForm = new FormGroup({
      'roleName': new FormControl(null, [Validators.required]),
      'description': new FormControl(null),
    });

    this.id = this.activtedroute.snapshot.params.id;
    console.log("id" + this.id);
    this.getRoleDetails(this.id);
  }

  //Getting Role Details for Selected Role
  getRoleDetails(id) {
    this.rolesService.getSelectedRole(id).subscribe(payloadResponse => {
      this.roleAddForm.get('roleName').setValue(payloadResponse['roleName']);
      this.roleAddForm.get('description').setValue(payloadResponse['description']);
    });
  }
  onSubmit() {
    var dataModel = new RoleEditRequest();

    //Added data into data model
    dataModel.RoleName = this.roleAddForm.value.roleName;
    dataModel.Description = this.roleAddForm.value.description;

    //Call API for Role Adding Data
    this.rolesService.editRole(this.id, dataModel).subscribe(
      response => {
        this.router.navigate(['/portalManagment/roleList']).then(this.appNotificationService.success('Role Updated Successfully!'));
      },
      (error) => {
        //Error handling
        this.apiErrorService.handleError(error, () => {
          this.appNotificationService.error("Role Failed to Create!");
        });
      });
  }
}























