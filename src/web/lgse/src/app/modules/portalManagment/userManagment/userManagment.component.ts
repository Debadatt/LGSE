import { Component, OnInit, NgModule, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { RolesService } from '../../../services/roles/roles.service';
import { AssigningRolesToUserRequest} from '../../../models/api/portalManagment/role.model';
import { UserService } from '../../../services/user.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-roleManagment',
  templateUrl: './userManagment.component.html',
  styleUrls: ['./userManagment.component.css']
})

export class UserManagmentComponent implements OnInit {

  //Variable Declaration
  roles: string[] = [];
  userroles: Userroles[];
  userrolesmap: Map<string, Userroles>;
  assigningRolesToUserRequest: AssigningRolesToUserRequest;
  id: string;
  Name: string;
  feature = FeatureNames;

  constructor(private router: Router,
    private userService: UserService,
    private rolesService: RolesService,
    private apiErrorService: ApiErrorService,
    private activtedroute: ActivatedRoute,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {

    this.id = this.activtedroute.snapshot.params.id;
    this.Name = this.activtedroute.snapshot.params.firstName + " " + this.activtedroute.snapshot.params.lastName;

    //Getting all Roles
    this.getRoleList();
  }

  //Getting Roles for Each User
  gettingRolesForUser() {
    this.rolesService.gettingUsersForRole(this.id).subscribe(payloadResponse => {
      for (let i = 0; i < payloadResponse.length; i++) {
        const role = this.userrolesmap.get(payloadResponse[i]['id']);
        if (role) {
          for (let i = 0; i < this.userroles.length; i++) {
            if (this.userroles[i].roleid == role.roleid) {
              //set true/false for every role for Perticular user
              this.userroles[i].checked = true;
            }
          }
        }
      }
    });
  }

  getRoleList() {
    //initially getting role List
    this.rolesService.getRoleDataList().subscribe(RoleListResponse => {
      this.userroles = [];
      for (let roles of RoleListResponse) {
        const role = new Userroles();
        role.checked = false;
        role.rolename = roles.roleName;
        role.roleid = roles.id;
        this.userroles.push(role);
      }

      //Existing Role assigned for Each user
      this.userrolesmap = new Map(this.userroles.map(x => [x.roleid, x] as [string, Userroles]))
      this.gettingRolesForUser();
    });
  }

  onSubmit() {

    //Added data into data model
    const dataModel = new AssigningRolesToUserRequest();
    dataModel.RoleIds = [];
    dataModel.UserId = this.id;
    let isarraychecked = false;
    for (let i = 0; i < this.userroles.length; i++) {
      if (this.userroles[i].checked) {
        isarraychecked = true;
        dataModel.RoleIds.push(this.userroles[i].roleid);
      }
    }
    //Validation
    if (!isarraychecked) {
      this.appNotificationService.error('Please Select At Least One Role');
      return;
    }
    //Call API for Role Adding Data
    this.rolesService.assigningRolesToUser(dataModel).subscribe(
      response => {
        this.router.navigate(['/portalManagment/userList']).then(this.appNotificationService.success('Roles Assigning To User Successfully!'));
      },
      (error) => {

        //Error Handling
        this.apiErrorService.handleError(error, () => {
          if (error.error.message == "EUSR_DOES_NOT_EXISTS") {
            this.appNotificationService.error("Please Add EUSR Field For Engineer Role!");
          }
        });
      });
  }
}
export class Userroles {
  rolename: string;
  roleid: string;
  checked: boolean;
}














