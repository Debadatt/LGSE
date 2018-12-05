import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { PropertiesService } from '../../../services/properties.service';
import { SubStausListResponse, UpdateMPRNStatusRequest } from '../../../models/api/properties.model';
import { PassIncidentData } from '../../../models/api/incident.model';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { FeatureNames, PathType } from '../../../app-common-constants';
import { PassdataService } from 'src/app/services/passdata.service';

@Component({
  selector: 'app-incident-update-status',
  templateUrl: './incident-update-status.component.html',
  styleUrls: ['./incident-update-status.component.css']
})

export class IncidentUpdateStatusComponent implements OnInit {

  //Variable Declaration
  receivedid: string;
  incidentid: string;
  recordid: string;
  incidentdbid: string;
  incidentUpdateStatusForm: FormGroup;
  formStatus = [];
  formSubStatus = [];
  subStausListResponse: SubStausListResponse[] = [];
  feature = FeatureNames;
  incidentstatus: number;
  subStatusCount: number;
  receivedpathtype: string;
  @ViewChild('status') status: ElementRef;


  constructor(private router: Router,
    private propertiesService: PropertiesService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService,
    private passdataservice: PassdataService,
    private activtedroute: ActivatedRoute) { }

  ngOnInit() {

    this.incidentUpdateStatusForm = new FormGroup({
      'status': new FormControl(null),
      'subStatus': new FormControl(null),
      'mprn': new FormControl(null),
      'buildingName': new FormControl(null),
      'buildingNumber': new FormControl(null),
      'zones': new FormControl(null),
      'cells': new FormControl(null),
      'principalStreet': new FormControl(null),
      'priorityCustomer': new FormControl(null),
      'notes': new FormControl(null)
    });

    //Getting property id
    this.receivedid = this.activtedroute.snapshot.params.propertyid;
    this.recordid = this.activtedroute.snapshot.params.incidentid;
    this.incidentdbid = this.activtedroute.snapshot.params.recordid;
    this.incidentstatus = this.activtedroute.snapshot.params.status;
    this.receivedpathtype = this.activtedroute.snapshot.params.urltype;
    this.subStatusCount = 0;
    this.getStatusList();
    this.getMprnData();
  }

  //Get Status List
  getStatusList() {
    this.propertiesService.getStatusDataList().subscribe(StatusListResponse => {
      for (let status of StatusListResponse) {
            console.log('get status list response success');
            this.formStatus.push({
              id: status.id,
              status: status.status
            });
      }
    });
  }

  //If any Status Changes SelectChangeHandler getting Called
  selectChangeHandler() {
    this.getSubStatusList("'" + this.incidentUpdateStatusForm.get('status').value + "'")
  }

  //Get SubStatus List--Need to Provide Status Id
  getSubStatusList(substatusid) {
    this.formSubStatus = [];
    this.subStatusCount = 0;
    this.propertiesService.getSubStatusDataList(substatusid).subscribe(subStausListResponse => {
      for (let subStatus of subStausListResponse) {
        this.formSubStatus.push({
          id: subStatus.id,
          subStatus: subStatus.subStatus
        });
        if (this.formSubStatus.length > 0) {
          this.subStatusCount = this.formSubStatus.length;
        }
      }
    });
  }

  //Get Properties/MPRN Details
  getMprnData() {
    this.propertiesService.getMprnData(this.receivedid).subscribe(payloadResponse => {
      this.incidentid = payloadResponse['incidentId'];
      this.incidentUpdateStatusForm.get('mprn').setValue(payloadResponse['mprn']);
      this.incidentUpdateStatusForm.get('buildingName').setValue(payloadResponse['buildingName']);
      this.incidentUpdateStatusForm.get('buildingNumber').setValue(payloadResponse['buildingNumber']);
      this.incidentUpdateStatusForm.get('zones').setValue(payloadResponse['zone']);
      this.incidentUpdateStatusForm.get('cells').setValue(payloadResponse['cell']);
      this.incidentUpdateStatusForm.get('principalStreet').setValue(payloadResponse['principalStreet']);
      if (payloadResponse['priorityCustomer'] == true) {
        this.incidentUpdateStatusForm.get('priorityCustomer').setValue('Yes');
      }
      else {
        this.incidentUpdateStatusForm.get('priorityCustomer').setValue('No');
      }
      this.incidentUpdateStatusForm.get('status').setValue(payloadResponse['latestStatusId']);
      this.selectChangeHandler();
      this.incidentUpdateStatusForm.get('subStatus').setValue(payloadResponse['latestSubStatusId']);
      this.incidentUpdateStatusForm.get('notes').setValue(payloadResponse['notes']);
    });
  }
  
  onSubmit() {
    const dataModel = new UpdateMPRNStatusRequest();
    //Added data into data model
    dataModel.PropertyId = this.receivedid;
    dataModel.StatusId = this.incidentUpdateStatusForm.value.status;
    if (this.subStatusCount > 0) {
      dataModel.PropertySubStatusMstrsId = this.incidentUpdateStatusForm.value.subStatus;
    }
    else {
      dataModel.PropertySubStatusMstrsId = null;
    }

    dataModel.Notes = this.incidentUpdateStatusForm.value.notes;
    this.propertiesService.updateMprnStatus(dataModel).subscribe(
      response => {
        const incident = new PassIncidentData();
        incident.incidentid = this.recordid;
        incident.recordid = this.incidentdbid;
        incident.status = this.incidentstatus;
        if (this.receivedpathtype === PathType.WITHOUT_PARAM) {
          this.router.navigate([this.passdataservice.backpath.path]).then(this.appNotificationService.success('Status Updated Successfully!'));
        } else {
          this.router.navigate([this.passdataservice.backpath.path, this.passdataservice.backpath.pathparams]).then(this.appNotificationService.success('Status Updated Successfully!'));
        }
      },
      (error) => {
        //Error Handling
        this.apiErrorService.handleError(error, () => {
          this.appNotificationService.error("Status Failed to Update!");
        });
      });
  }
  cancel() {
    if (this.receivedpathtype === PathType.WITHOUT_PARAM) {
      this.router.navigate([this.passdataservice.backpath.path]);
    } else {
      this.router.navigate([this.passdataservice.backpath.path,this.passdataservice.backpath.pathparams]);
    }
  }
}
