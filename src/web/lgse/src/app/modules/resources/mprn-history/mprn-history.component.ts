import { Component, OnInit, ViewChild, NgModule } from '@angular/core';
import { ActivatedRoute,Route, Router } from '@angular/router';
import { AssignedMPRNResponse } from 'src/app/models/api/resources/assigned-mprn-response';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from 'src/app/models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from 'src/app/services/resources/resources-service.service';
import { forEach } from '@angular/router/src/utils/collection';
import { MPRNAssignmentUnassignment } from 'src/app/models/api/resources/assign-unassign-mprn-request';
import { AppNotificationService } from 'src/app/services/notification/app-notification.service';
import { ApiErrorService } from 'src/app/services/api-error.service';
import { GetActiveMPRnList } from 'src/app/models/api/resources/get-active-mprn-response';
import { MPRNIdLists } from 'src/app/models/api/resources/mprn-id-list';
import { MPRNDetails } from 'src/app/models/api/resources/paas-resources-data';
import { PassdataService } from 'src/app/services/passdata.service';
import { CustomSearchData, CustomSearchDataMain } from 'src/app/models/search/custom-search-data';
import { MprnhistoryResponse } from 'src/app/models/api/resources/mprn-history-response';
import { MAP_TYPE, PathType } from 'src/app/app-common-constants';



@Component({
  selector: 'app-mprn-history',
  templateUrl: './mprn-history.component.html',
  styleUrls: ['./mprn-history.component.css']
})
export class MprnHistoryComponent implements OnInit {
  propertyid: string;
  mprnnumber: string;
  incidentstatus:string;
  mprnhistorylist: MprnhistoryResponse[] = [];
  displayedColumns = ['name', 'email', 'roleName', 'status', 'subStatus', 'statusChangedOn', 'notes'];

  dataSource;
  constructor(
    private apiErrorService: ApiErrorService,
    private activateroute: ActivatedRoute,
    private router:Router,
    private passdataservice: PassdataService,
    private resourcesServiceService: ResourcesServiceService,
    private appNotificationService: AppNotificationService
  ) { }

  ngOnInit() {
    this.propertyid = this.activateroute.snapshot.params.propertyid;
    this.mprnnumber=this.activateroute.snapshot.params.mprn;
    this.incidentstatus=this.activateroute.snapshot.params.status;
    console.log('this.propertyid', this.propertyid,this.mprnnumber);
    this.getMPRNHistory();
  }
  // code block for getting history of MPRN
  getMPRNHistory(): void {
    this.resourcesServiceService.getMPRNHistoryList(this.propertyid).subscribe(
      response => {
        console.log('mprn history response.');
        console.log(response);
        this.mprnhistorylist = response;
        this.dataSource = new MatTableDataSource<MprnhistoryResponse>(this.mprnhistorylist);
      },
      (error) => {
        this.apiErrorService.handleError(error);
      });

  } // end of fucntion.
  back()
  {
    if (this.passdataservice.backpath.pathtype == PathType.WITHOUT_PARAM) {
      this.router.navigate([this.passdataservice.backpath.path]);
    } else {
      this.router.navigate([this.passdataservice.backpath.path, this.passdataservice.backpath.pathparams]);
    }
  }


  // end of code block
}
