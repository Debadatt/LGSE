import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { DomainService } from '../../../../services/domains/domain.service';
import { DomainEditRequest } from '../../../../models/api/portalManagment/domain.model';
import { ApiErrorService } from '../../../../services/api-error.service';
import { AppNotificationService } from '../../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';

@Component({
  selector: 'app-whiteListDomain-edit',
  templateUrl: './whiteListDomain-edit.component.html',
  styleUrls: ['./whiteListDomain-edit.component.css']
})

export class WhiteListDomainEditComponent implements OnInit {
  
  //variable Declaration
  id: string;
  domainAddForm: FormGroup;
  status: string;
  feature = FeatureNames;

  constructor(private router: Router,
    private domainService: DomainService,
    private apiErrorService: ApiErrorService,
    private activtedroute: ActivatedRoute,
    public localstorageservice: LocalstorageService,
    private appNotificationService: AppNotificationService) { }

  ngOnInit() {
    this.domainAddForm = new FormGroup({
      'domainName': new FormControl(null, [Validators.required]),
      'organisation': new FormControl(null, [Validators.required]),
      'status': new FormControl(null),
    });

    //Get Selected Domain
    this.id = this.activtedroute.snapshot.params.id;
    this.getDomainDetails(this.id);
  }

  getDomainDetails(id) {
    let isActive: boolean;
    this.domainService.getSelectedDomain(id).subscribe(payloadResponse => {
      this.domainAddForm.get('domainName').setValue(payloadResponse['domainName']);
      this.domainAddForm.get('organisation').setValue(payloadResponse['orgName']);
      this.domainAddForm.get('status').setValue(payloadResponse['isActive']);
      isActive = payloadResponse['isActive'];

      //If Domain Active--set 1
      if (isActive == true) {
        this.domainAddForm.get('status').setValue("1");
      }
      else {
        this.domainAddForm.get('status').setValue("0");
      }
    });
  }
  onSubmit() {
    var dataModel = new DomainEditRequest();

    //Added data into data model
    dataModel.orgName = this.domainAddForm.value.organisation;
    if (this.domainAddForm.get('status').value == 1) {
      dataModel.isActive = true;
    }
    else {
      dataModel.isActive = false;
    }

    //Call API for Domain Editing Data
    this.domainService.editDomain(this.id, dataModel).subscribe(
      response => {
        this.router.navigate(['/portalManagment/whiteListDomain']).then(this.appNotificationService.success('Domain Updated Successfully!'));
      },
      (error) => {

        //Error Handling
        this.apiErrorService.handleError(error, () => {
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
            this.appNotificationService.error("Domain Failed To Update!");
          }
        });
      });
  }
}

