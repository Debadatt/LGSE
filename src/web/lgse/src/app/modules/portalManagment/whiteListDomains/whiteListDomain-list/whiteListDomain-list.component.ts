import { Component, OnInit,ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { DomainService } from '../../../../services/domains/domain.service';
import { DomainListResponse } from '../../../../models/api/portalManagment/domain.model';
import { PassdataService } from '../../../../services/passdata.service';
import { CustomSearchData, CustomSearchDataMain } from 'src/app/models/search/custom-search-data';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-whiteListDomain-list',
  templateUrl: './whiteListDomain-list.component.html',
  styleUrls: ['./whiteListDomain-list.component.css']
})

export class WhiteListDomainComponent implements OnInit {
   displayedColumns = ['domainName', 'orgName','isActive', 'action'];

   //Variable Declaration
   customSearchDataMain = new CustomSearchDataMain();
   selectedresourcetype: string;
   customSearchData: CustomSearchData[] = [];
   selected: string[] = [];
   dataSource;
   isvisible = false;
   isExpanded = false;
   count = 0;
   feature = FeatureNames;

   domainListResponse:DomainListResponse[]=[];
   @ViewChild(MatPaginator) paginator: MatPaginator;
   @ViewChild(MatSort) sort: MatSort;

    constructor(
    private router: Router,
    private domainService: DomainService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    private passdataservice: PassdataService,
    public localstorageservice: LocalstorageService
  ) { }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
    ngOnInit() {
      this.getDomainList();
    }
   
    //Get Domain List
    getDomainList() {
      this.domainService.getDomainList().subscribe(payloadResponse => {
        this.domainListResponse=payloadResponse;
        this.dataSource = new MatTableDataSource<DomainListResponse>(this.domainListResponse);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
      });
    }

    //Edit Domain link
    editDomain(id): void {
      this.router.navigate(['/portalManagment/editWhiteListDomain',id]);
    }

    //Add New Domain Link
    addDomain(): void {
      this.router.navigate(['/portalManagment/addWhiteListDomain']);
    }

    // code block for Refresh Button
    refresh(): void {
    this.getDomainList();
    } 
  }





 














 









