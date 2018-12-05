import { Component, OnInit,ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../../services/user.service';
import { UserListResponse,PassUserData } from '../../../../models/api/user.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { PassdataService } from '../../../../services/passdata.service';
import { BackButtonPath } from 'src/app/models/ui/back-btutton'; 
import {  PathType, FeatureNames } from '../../../../app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})

export class UserListComponent implements OnInit {
  displayedColumns = ['firstName', 'lastName', 'roles','employeeId','eusr','contactNo','email','isActive','action'];

  //Variable Declaration
  dataSource;
  isvisible = false;
  isExpanded = false;
  count = 0;
  feature = FeatureNames;


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private router: Router,
    private userService: UserService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    private passdataservice: PassdataService,
    public localstorageservice: LocalstorageService
  ) { }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
    ngOnInit() {
      this.getUserList();
    }

    //Get All Users
    getUserList() {
      this.userService.getUserList().subscribe(payloadResponse => {
        this.dataSource = new MatTableDataSource<UserListResponse>(payloadResponse);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
    }

    //Edit Role
     editUser(id): void {
       // set object in service for back button
      const backButtonPath = new BackButtonPath<null>();
      backButtonPath.pathtype = PathType.WITHOUT_PARAM;
      backButtonPath.path = '/portalManagment/userList';
      this.passdataservice.backpath = backButtonPath;
      this.router.navigate(['/portalManagment/editUser',id]);
    }

    assignUserToRoles(id,firstName,lastName): void {

      //Assigning the Users to Role
      const userData = new PassUserData();
      userData.id = id;
      userData.firstName = firstName;
      userData.lastName=lastName;
      this.router.navigate(['/portalManagment/assignUserToRoles',userData]);
    }
    //Add New User
    addUser(): void {
      this.router.navigate(['/portalManagment/addUser']);
    }
    // code block for Refresh Button
    refresh(): void {
    this.getUserList();
    }
  
  }

  
  
  
  
  
   
  
  
  
  
  
  
  
  
  
  
 









