import { Component, OnInit, ViewChild, NgModule } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from 'src/app/models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from 'src/app/services/resources/resources-service.service';
import { ActivatedRoute } from '@angular/router';
import { MPRNAssignmentUnassignment } from 'src/app/models/api/resources/assign-unassign-mprn-request';
import { AppNotificationService } from 'src/app/services/notification/app-notification.service';
import { Router } from '@angular/router';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-view-assigned-resources-to-mprn',
  templateUrl: './view-assigned-resources-to-mprn.component.html',
  styleUrls: ['./view-assigned-resources-to-mprn.component.css']
})
export class ViewAssignedResourcesToMprnComponent implements OnInit {

  //displayedColumns = ['name'];
  displayedColumns = ['select', 'mprn', 'name', 'AssignedMPRNCount', 'roleName', 'Zones', 'Cells', 'Completed', 'email'];
  selectedresourcetype: string;
  dataSource;
  receivedpropertyid: string;
  feature = FeatureNames;
  selectall: boolean;
  isinprogress = false;
  mprnAssignmentUnassignment: MPRNAssignmentUnassignment[] = []
  receivedmprn: string;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  // process varibales 
  getAllResourcesResponse: GetAllResourcesResponse[] = [];

  // end of process variables
  constructor(
    private resourcesServiceService: ResourcesServiceService,
    private activatedrouter: ActivatedRoute,
    private router: Router,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) {

  }
  ngOnInit() {


    this.receivedpropertyid = this.activatedrouter.snapshot.params.propertyid;
    this.receivedmprn = this.activatedrouter.snapshot.params.mprn;
    console.log('received property id', this.receivedpropertyid);
    this.getResources(); // function call for getting resources
  } // end of ng init
  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //end filter


  // fucntion for getting all resources from server
  getResources(): void {
    this.resourcesServiceService.getResourceListofMPRN(this.receivedpropertyid).subscribe((getresourcePayloadresponse) => {
      this.getAllResourcesResponse = getresourcePayloadresponse;
      this.dataSource = new MatTableDataSource<GetAllResourcesResponse>(this.getAllResourcesResponse);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    });
    console.log(this.getAllResourcesResponse);
  }
  // end of code block
  // code block for unaasign resource from mprn
  unssignSelected(): void {
    this.mprnAssignmentUnassignment = [];
    for (let mprn of this.getAllResourcesResponse) {
      if (mprn.checked) {
        const mprnobject = new MPRNAssignmentUnassignment();
        mprnobject.IsUnAssign = true;
        mprnobject.UserId = mprn.id;
        mprnobject.PropertyId = mprn.propertyId;
        mprnobject.RoleId = mprn.roleId;
        this.mprnAssignmentUnassignment.push(mprnobject);
      }
    }
    console.log('this.mprnAssignmentUnassignment');
    console.log(this.mprnAssignmentUnassignment);
    if (this.mprnAssignmentUnassignment.length <= 0) {
      this.appNotificationService.error("Select Resources To Unassign");
      return;
    }
    console.log('resource unassignment array of objects');
    console.log(this.mprnAssignmentUnassignment);
    this.isinprogress = true;
    this.resourcesServiceService.assignUnassignresources(this.mprnAssignmentUnassignment).subscribe(
      response => {
        this.isinprogress = false;
        this.router.navigate(['/resources/assign-mprn'], { skipLocationChange: true }).then(this.appNotificationService.success('Resource Unassigned Successfully!'));
      },
      (error) => {
        this.isinprogress = false;
        this.apiErrorService.handleError(error);
      });

  }
  //end code block.
  // code block for select all
  selectAlll() {
    console.log('select all', this.selectall);
    for (let mprn of this.getAllResourcesResponse) {
      mprn.checked = this.selectall;
    }

  }
  //end of block
  // fucntion for ploat cell and zone
  cellZone(Data: any): string {
    let datastring = '';
    for (let i = 0; i < Data.length; i++) {
      const val = Data[i].trim();
      if (i > 0 && val != '' && Data[i - 1].trim() != '') {
          datastring += ',' + val;
      } else {
        datastring += val;
      }
    }
    return datastring;
  }
  // end of fucntion
}// end of class 
