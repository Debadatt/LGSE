import { Component, OnInit, ViewChild, NgModule } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { GetAllResourcesResponse } from 'src/app/models/api/resources/get-all-resources-response';
import { ResourcesServiceService } from 'src/app/services/resources/resources-service.service';
import { GetAssignedResource } from 'src/app/models/api/resources/get-assigned-resource';
import { Router } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { PassResourceData } from 'src/app/models/api/resources/paas-resources-data';

@Component({
  selector: 'app-assigned-resources',
  templateUrl: './assigned-resources.component.html',
  styleUrls: ['./assigned-resources.component.css']
})
export class AssignedResourcesComponent implements OnInit {
  // displayedColumns = ['name'];
  displayedColumns = ['name', 'role', 'organization', 'empid', 'email', 'inprogress', 'completed', 'action'];
  selectedresourcetype: string;
  getAssignedResource: GetAssignedResource[] = [];
  dataSource;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(
    private resourcesServiceService: ResourcesServiceService,
    private router: Router) { }

  ngOnInit() {
    this.getAssignedresources();// fucntion call for getting assignned resources.
  } // end of ng init.

  // fucntion fot apply filetr
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  //end filter


  // code block for getting assigned resources 
  getAssignedresources(): void {
    this.resourcesServiceService.getAssinedResourceList().subscribe((getresourcePayloadresponse) => {
      this.dataSource = new MatTableDataSource<GetAllResourcesResponse>(getresourcePayloadresponse);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    });

    // for (let i = 0; i <= 10; i++) {
    //   this.getAssignedResource.push({ id: i.toString(), firstName: 'Raj', lastName: 'ghadagge', role: 'engineer', organization: 'Sagacity',employeeId: 'SAG:' + i, email: 'ghadage_raj@yahoo.in', inprogress: i,completed: 1 });
    // }
    // this.dataSource = new MatTableDataSource<GetAssignedResource>(this.getAssignedResource);
    // this.dataSource.sort = this.sort;
    // this.dataSource.paginator = this.paginator;
    console.log('get all resource list');
    console.log(this.getAssignedResource);
  }
  //end of code block
  // code block for view MPRN of selcted resources
  viewSelctedresourcesMPRN(resourceemail, resourceid): void {
    const passdata = new PassResourceData();
    passdata.email = resourceemail;
    passdata.id = resourceid;
    console.log('selevcted resources email id', resourceemail);
    this.router.navigate(['resources/assignedmprn', passdata], { skipLocationChange: true });
    // this.router.navigate(['incident/showProperties', "'" + id + "'"], { skipLocationChange: true });
  }
  //end of code block

}
