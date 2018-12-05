import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { RolesService } from '../../../services/roles/roles.service';
import { AssigningUsersToRoleRequest } from '../../../models/api/portalManagment/role.model';
import { UserService } from '../../../services/user.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-roleManagment',
  templateUrl: './roleManagment.component.html',
  styleUrls: ['./roleManagment.component.css']
})

export class RoleManagmentComponent implements OnInit {

  //Variable Declaration
  users: string[] = [];
  userroles: Userroles[];
  userrolesmap: Map<string, Userroles>;
  roleManagmentForm: FormGroup;
  assigningUsersToRoleRequest: AssigningUsersToRoleRequest;
  formUsers = [];
  dataSource;
  id: string;
  roleName: string;
  feature = FeatureNames;

  constructor(private router: Router,
    private userService: UserService,
    private rolesService: RolesService,
    private apiErrorService: ApiErrorService,
    private activtedroute: ActivatedRoute,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService) { }
  
    ngOnInit() {
    this.id = this.activtedroute.snapshot.params.roleId;
    this.roleName=this.activtedroute.snapshot.params.roleName;
    //getting User List
    this.getUserList();
  }

  gettingUsersForRole() {
    //Assign Users to each Role
    this.rolesService.gettingRolesForUser(this.id).subscribe(payloadResponse => {
      for (let i = 0; i < payloadResponse.length; i++) {
        const user = this.userrolesmap.get(payloadResponse[i]['id']);
        if (user) {
          for (let i = 0; i < this.userroles.length; i++) {
            if (this.userroles[i].userid == user.userid) {
              this.userroles[i].checked = true;
            }
          }
        }
      }
    });

  }
  getUserList() {
    this.userService.getUserList().subscribe(UserListResponse => {
      this.userroles = [];
      for (let users of UserListResponse) {
        const user = new Userroles();
        user.checked = false;
        user.username = users.firstName+" "+users.lastName;
        user.userid = users.id;
        user.email=users.email;
        this.userroles.push(user);
      }
      this.userrolesmap = new Map(this.userroles.map(x => [x.userid, x] as [string, Userroles]))
      //Assigning Users for Each Role
      this.gettingUsersForRole();
    });
  }
  onSubmit() {
    const dataModel = new AssigningUsersToRoleRequest();
    //Added data into data model
    dataModel.UserIds = [];
    dataModel.RoleId = this.id;
    let isarraychecked = false;// dataModel.RoleId = this.id;
    for (let i = 0; i < this.userroles.length; i++) {
      if (this.userroles[i].checked) {
        isarraychecked = true;
        dataModel.UserIds.push(this.userroles[i].userid);
      }
    }
    if (!isarraychecked) {
      this.appNotificationService.error('Please Select At Least One User');
      return;
    }
    this.rolesService.assigningUsersToRole(dataModel).subscribe(
      response => {
        this.router.navigate(['/portalManagment/roleList']).then(this.appNotificationService.success('Roles Assigning To User Successfully!'));
      },
      (error) => {
        //Error Handling
          this.apiErrorService.handleError(error, () => {
            if (error.error.message == "EUSR_DOES_NOT_EXISTS") {
              this.appNotificationService.error("Please Add EUSR Field!");
      }
        });
      });
  }
} // end of class


export class Userroles {
  username: string;
  userid: string;
  email:string;
  checked: boolean;
}














