import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DomainService } from '../../../../services/domains/domain.service';
import { DomainAddRequest } from '../../../../models/api/portalManagment/domain.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-whiteListDomain-addEdit',
  templateUrl: './whiteListDomain-addEdit.component.html',
  styleUrls: ['./whiteListDomain-addEdit.component.css']
})

export class WhiteListDomainAddEditComponent implements OnInit {

  //Variable Declaration
  domainAddForm: FormGroup;
  feature = FeatureNames;

  constructor(private router: Router,
    private domainService: DomainService,
    private apiErrorService: ApiErrorService,
    public localstorageservice: LocalstorageService,
    private appNotificationService: AppNotificationService) { }

  ngOnInit() {
    this.domainAddForm = new FormGroup({
      'domainName': new FormControl(null, [Validators.required]),
      'organisation': new FormControl(null, [Validators.required]),
    });
  }

  onSubmit() {
    var dataModel = new DomainAddRequest();

    //Added data into data model
    dataModel.OrgName = this.domainAddForm.value.organisation;
    dataModel.DomainName = this.domainAddForm.value.domainName;
    dataModel.IsActive = true;

    //Call API For Add Domain
    this.domainService.addDomain(dataModel).subscribe(
      response => {
        //Suceess Then Transfer to Domain List
        this.router.navigate(['/portalManagment/whiteListDomain']).then(this.appNotificationService.success('Domain Created Successfully!'));
      },
      (error) => {
        this.apiErrorService.handleError(error, () => {
          //Error Message Handling
          if (error.error.message == "DOMAIN_EXISTS_ALREADY") {
            this.appNotificationService.error("Domain Already Exist!");
          }
          else if (error.error.message == "ORG_EXISTS_ALREADY") {
            this.appNotificationService.error("Organisation Already Exist!");
          }
          else if (error.error.message == "DOMAIN_IS_INACTIVE") {
            this.appNotificationService.error("Domain Is Inactive!");
          }
          else {
            this.appNotificationService.error("Domain Failed To Create!");
          }
        });
      });
  }
}

