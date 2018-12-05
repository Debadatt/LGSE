import { Component, OnInit, ViewChild, NgModule } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from '../../../models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from '../../../services/resources/resources-service.service';
import { Subscription } from 'rxjs/internal/Subscription';
import { ActivatedRoute } from '@angular/router';
import { MPRNAssignmentUnassignment } from '../../../models/api/resources/assign-unassign-mprn-request';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { MPRNIdLists } from '../../../models/api/resources/mprn-id-list';
import { ApiErrorService } from '../../../services/api-error.service';
import { Router } from '@angular/router';
import { CustomSearchData, CustomSearchSubSelectionValues, CustomSearchDataMain, SearchFilter } from 'src/app/models/search/custom-search-data';
import { PassdataService } from 'src/app/services/passdata.service';
import { merge, Observable } from 'rxjs';
import { startWith } from 'rxjs/operators';
import { switchMap } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { EventEmitter } from '@angular/core';
import { FeatureNames, ResourcesType } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { GetActiveMPRnList } from 'src/app/models/api/resources/get-active-mprn-response';
@Component({
  selector: 'app-assign-resource-mprn',
  templateUrl: './assign-resource-mprn.component.html',
  styleUrls: ['./assign-resource-mprn.component.css']
})
export class AssignResourceMprnComponent implements OnInit {
  activatedroutesubscriptions: Subscription;
  //displayedColumns = ['name'];
  displayedColumns = ['select', 'FirstName', 'AssignedMPRNCount', 'role', 'Zone', 'Cell', 'Completed'];
  selectedresourcetype: string;
  customSearchDataMain = new CustomSearchDataMain();
  customSearchData: CustomSearchData[] = [];
  isvisible = false;
  selected: string[] = [];
  isinprogress = false;
  searchFilter: SearchFilter[] = [];
  dataSource;
  customSearchSubSelectionValues: CustomSearchSubSelectionValues[] = [];
  // incidentListResponse1: IncidentListResponse[] = [];


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  // process varibales 
  getAllResourcesResponse: GetAllResourcesResponse[] = [];
  selectedusersformprn: AssignRoles[] = [];;
  mprnAssignmentUnassignment: MPRNAssignmentUnassignment[] = [];
  mPRNIdLists: MPRNIdLists[] = [];
  receivedmprns: GetActiveMPRnList[] = [];
  refreshTable: EventEmitter<any> = new EventEmitter();
  resultsLength = 0;
  feature = FeatureNames;
  ispropertyisolated = false;
  isisolatorselected = false;
  selectall = false;
  // end of process variables
  constructor(
    private resourcesServiceService: ResourcesServiceService,
    private apiErrorService: ApiErrorService,
    private router: Router,
    private passdataservice: PassdataService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService,
    private activatedroute: ActivatedRoute) { }
  ngOnInit() {
    this.searchKeyValue();

    this.receivedmprns = this.passdataservice.selectedmprnllist;
    // code block for  get custom serach data
    this.activatedroutesubscriptions = this.passdataservice.customsearchvalue.subscribe((val) => {
      console.log('custom search data received');
      console.log(val);
      this.searchFilter = val;
      this.refreshTable.emit(null);
    });  //end

    // ==================================== paginator starts =============================
    // set default load parameters
    this.paginator.pageSize = 50;
    this.sort.active = 'FirstName';
    this.sort.direction = 'asc';

    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.refreshTable.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page, this.refreshTable)
      .pipe(
      startWith({}),
      switchMap(() => {
        console.log('in switch map');
        console.log(this.sort.active)
        return this.getResources();
      }),
      map(data => {
        // Flip flag to show that loading has finished.     
        return data;
      }),
    ).subscribe(
      resourcePayloadResponse => {
        console.log('response', resourcePayloadResponse);
        console.log('Bank list  Response succeed');
        // this.resultsLength = bankListPayloadResponse.data.filteredrecords;
        //  this.dataSource = bankListPayloadResponse.data.records;
        if (resourcePayloadResponse && resourcePayloadResponse !== null) {
          this.resultsLength = resourcePayloadResponse.count;
          this.getAllResourcesResponse = resourcePayloadResponse.results;
          this.dataSource = new MatTableDataSource<GetAllResourcesResponse>(this.getAllResourcesResponse);
          console.log(this.dataSource);
        }
      });
    this.selectedresourcetype = '1';
    //  this.getResources(this.selectedresourcetype); // function call for getting resources
  } // end of ng init





  // pass search key data to custom search

  searchKeyValue(): void {
    this.customSearchData.push({
      searchkeydisplayname: 'First Name',
      searchkey: 'FirstName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Last Name',
      searchkey: 'LastName',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Email Id',
      searchkey: 'Email',
      controltype: 'input',
      controlvalues: null
    });
    this.customSearchData.push({
      searchkeydisplayname: 'Role',
      searchkey: 'PreferredRole',
      controltype: 'Select',
      controlvalues: [
        { displaytext: 'Engineer', value: 'Engineer' },
        { displaytext: 'Isolator', value: 'Isolator' }
      ]
    });
    const selectData = ['FirstName', 'LastName'];

    this.customSearchDataMain.selected = selectData;
    this.customSearchDataMain.fieldlist = this.customSearchData;
  }// end of fucntion
  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //end filter
  getResources(): Observable<any> {
    return this.resourcesServiceService.getResourceList(this.paginator.pageIndex, this.paginator.pageSize, this.sort.active, this.sort.direction, this.searchFilter);
  }

  // fucntion for getting all resources from server

  // end of code block
  // function for getting resources based on radio buttons selection.
  getresourcesfucn() {
    console.log('selected resources types');
    console.log(this.selectedresourcetype);
    // this.getResources(this.selectedresourcetype);
  }
  // code block for assigning resources to mprns
  assignMPRNToresource(): void {
    // checking if user has selected islolated property to assign
    this.ispropertyisolated = false;
    for (let k = 0; k < this.receivedmprns.length; k++) {
      if (this.receivedmprns[k].isIsolated) {
        this.ispropertyisolated = true;
        break;
      }
    } // end of block.
    // for getting selected resources list
    this.selectedusersformprn = [];
    for (let mprn of this.getAllResourcesResponse) {
      if (mprn.checked) {
        this.selectedusersformprn.push({ id: mprn.id, roleid: mprn.preferredRoleId, rolename: mprn.preferredRole });
      }
    }// end of block.
    // if property is isolated
    if (this.ispropertyisolated) {
      this.isisolatorselected = false;
      for (let mprn = 0; mprn < this.selectedusersformprn.length; mprn++) {
        if (this.selectedusersformprn[mprn].rolename === ResourcesType.ISOLATOR) {
          this.isisolatorselected = true;
          break;
        }
      } // end of loop
      if (this.isisolatorselected) {
        this.appNotificationService.error('Isolator cannot receive Restore Jobs');
      } else {
        this.saveAssignedresources();
      }
    } else {
      this.saveAssignedresources();
    }
  } // end of code block
  saveAssignedresources() {
    if (this.selectedusersformprn.length <= 0) {
      this.appNotificationService.error("Select ResourcesResources To Assign MPRN");
      return;
    } else {
      this.mprnAssignmentUnassignment = [];
      // list of id's of Selcted MPRNS
      for (let i = 0; i < this.receivedmprns.length; i++) {
        // list of id's of selected resources 
        for (let j = 0; j < this.selectedusersformprn.length; j++) {
          const mprnobject = new MPRNAssignmentUnassignment();
          mprnobject.IsUnAssign = false;
          mprnobject.UserId = this.selectedusersformprn[j].id;
          mprnobject.PropertyId = this.receivedmprns[i].id;
          mprnobject.RoleId = this.selectedusersformprn[j].roleid;
          this.mprnAssignmentUnassignment.push(mprnobject);
        } // end of second loop
      } // end of frist loop
      console.log('Final array of objects');
      console.log(this.mprnAssignmentUnassignment);
      this.isinprogress = true;
      this.resourcesServiceService.assignUnassignresources(this.mprnAssignmentUnassignment).subscribe(
        response => {
          this.isinprogress = false;
          this.router.navigate(['/resources/assign-mprn'], { skipLocationChange: true }).then(this.appNotificationService.success('MPRN Assigned Successfully!'));
        },
        (error) => {
          this.isinprogress = false;
          this.apiErrorService.handleError(error);
        });
    }
  }
  //fucntiuon for show hide resources
  hideShowSeacrh(): void {
    console.log('button clicked');
    this.isvisible = true;
    this.passdataservice.setOpenSearch(true);

  }//end of fucntion
  // code block for select all
  selectAlll() {
    console.log('select all', this.selectall);
    for (let mprn of this.getAllResourcesResponse) {
      mprn.checked = this.selectall;
    }

  }
  //end of block
  // cleanup
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    if (this.activatedroutesubscriptions) {
      this.activatedroutesubscriptions.unsubscribe();
    }
  }// end of fucntion
  // code block for Refresh Button
  refresh(): void {
    this.refreshTable.emit(null);
  }
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

} // end of class
export class AssignRoles {
  id: string;
  roleid: string;
  rolename: string;
}