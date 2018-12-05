import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { IncidentService } from '../../../services/incident.service';
import { IncidentPropertiesRequest, IncidentEditRequest, IncidentResolveUsersRequest } from '../../../models/api/incident.model';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { ProcessRequest } from '../../../models/api/process.request.model';
import { CsvHeareds } from 'src/app/app-common-constants';
import { DuplicateIncidentRec } from 'src/app/models/ui/incident/duplicate-incident-rec';

@Component({
  selector: 'app-incident-edit',
  templateUrl: './incident-edit.component.html',
  styleUrls: ['./incident-edit.component.css']
})

export class IncidentEditComponent implements OnInit {

  //Variable Declaration
  incidentEditForm: FormGroup;
  duplicateIncidentRec: DuplicateIncidentRec[]; C
  propertyModel = new IncidentPropertiesRequest();
  formCategories = [];
  formStatus = [];
  users = [];
  mprnFileinJSONFormat: any;
  id: string;
  title: string;
  modalbody: string;
  modalfor: string;
  incidentNoOfProperies: string;
  incidentNoOfZones: string;
  incidentNoOfCells: string;
  noOfProperties: number;
  noOfZones: number;
  noOfCells: number;
  resolveUser = new IncidentResolveUsersRequest();
  emailIds = [];
  selectedUser: string;
  selectedName: string;
  dataModel = new IncidentEditRequest();
  Properties: IncidentPropertiesRequest[] = [];
  resolveUsers: IncidentResolveUsersRequest[] = [];
  uploadbuttonvisible = false;
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('opendialog') opendialog: ElementRef;
  customstatus: number;
  //Declaration End

  constructor(private router: Router,
    private incidentService: IncidentService, private activtedroute: ActivatedRoute,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService) { }

  ngOnInit() {
    //Incident Status
    this.formStatus = [{
      id: 0,
      name: 'In Progress',
      value: 0
    }, {
      id: 1,
      name: 'Complete',
      value: 1
    }, {
      id: 2,
      name: 'Cancel',
      value: 2
    }]

    this.incidentEditForm = new FormGroup({
      'incidentId': new FormControl(null),
      'description': new FormControl(null, [Validators.required]),
      'category': new FormControl(null, [Validators.required]),
      'notes': new FormControl(null),
      'noOfProperties': new FormControl(null),
      'noOfZones': new FormControl(null),
      'noOfCells': new FormControl(null),
      'noOfPropertiesRestoared': new FormControl(null),
      'uploadAffectedProperties': new FormControl(null),
      'status': new FormControl(null),
    });
    this.id = this.activtedroute.snapshot.params.id;
    this.getCategoryList();
    this.getIncidentData();
  }

  //Conversion of CSV fil to JSON Object
  private csvJSON(csv) {
    // spliting file from
    console.log('in json to csv');
    // saparating hearders and data in saparate line.
    var linesRead = csv.split("\r\n");
    // validation for blank csv
    if (linesRead.length === 0) {
      this.appNotificationService.error("You Can Not Upload Blank File");
      return;
    }
    var result = [];
    // create hearders from csv
    var headers = linesRead[0].split(",");
    let invalidheaders = false;
    // check headers validaty
    for (const header in headers) {
      const item = CsvHeareds.filter(commanheader => commanheader == headers[header])[0];
      if (!item) {
        invalidheaders = true;
        break;
      }
    }
    // if heareds are invalid then throw an errror
    if (invalidheaders) {
      this.appNotificationService.error("You have entered invalid headers in CSV");
      return;
    }
    const lines = [];
    // cheking blank line in csv if it is then break from line.
    for (let lineslen = 1; lineslen < linesRead.length; lineslen++) {
      if (linesRead[lineslen] == '') {
        break;
      }
      else {
        lines.push(linesRead[lineslen]);
      }
    }
    if (lines.length == 0) {
      this.appNotificationService.error("You Can Not Upload Blank File");
      return;
    }
    else {
      //Headers and data pushed to result
      for (var i = 0; i < lines.length; i++) {
        var obj = {};
        var currentline = lines[i].split(",");
        let testarray = [];
        let testboj = {};
        for (var j = 0; j < headers.length; j++) {
          obj[headers[j]] = currentline[j];
        }
        result.push(obj);
      }
    }
    //Returning JSON format
    console.log('csv too json', result);
    return result;
  }// end of fucntion.

  //Upload Csv File
  public uploadCsvFile() {
    this.noOfProperties = 0;
    this.noOfZones = 0;
    this.noOfCells = 0;
    var file = (<HTMLInputElement>document.getElementById('fileInput')).files[0];
    console.log(file);
    if (file != undefined && file !== null) {
      if (file.type.includes("excel")) {
        const reader = new FileReader();
        reader.onload = () => {
          let text = reader.result;
          //convert text to json here
          this.mprnFileinJSONFormat = this.csvJSON(text);
          // TODO: call only if the conversion to JSON is successful.
          //Calucalte No. Of Properties, Zone and cell from CSV File
          if (this.mprnFileinJSONFormat) {
            this.calculateCSVSummary();
          }
        };
        reader.readAsText(file);
      } else {
        this.appNotificationService.error("Upload CSV File");
      }
    }

  }

  //Calculate Count for No Of Properties,Zone and Cell
  calculateCSVSummary() {
    this.incidentEditForm.get('noOfProperties').setValue(this.mprnFileinJSONFormat.length);
    this.incidentEditForm.get('noOfZones').setValue(this.mprnFileinJSONFormat.map(item => item.Zone)
      .filter((value, index, self) => self.indexOf(value) === index).length);
    this.incidentEditForm.get('noOfCells').setValue(this.mprnFileinJSONFormat.map(item => item.Cell)
      .filter((value, index, self) => self.indexOf(value) === index).length);
  }

  //Check Duplicate Record
  checkDuplicateRecord() {
    // code for checking is user choose email for every user or not
    let selectionmissing = false;
    console.log('this.selected emails', this.duplicateIncidentRec);
    for (let userrec of this.duplicateIncidentRec) {
      if (userrec.selecteditem == undefined) {
        selectionmissing = true;
        break;
      }
    }
    if (selectionmissing) {
      this.appNotificationService.error('Select Email Of User');
      return;
    }
    // end of code

    //If User Name(Zone Manager/Cell Manager) Is Duplicate(same) and Email Id Is Different
    var selectedRecord: string;
    selectedRecord = this.selectedUser;
    this.dataModel.ResolveUsers = [];
    // pushing selected data to in API input
    for (let userrec of this.duplicateIncidentRec) {
      if (userrec.selecteditem) {
        this.resolveUsers.push(userrec.selecteditem);
      }
    }
    this.dataModel.ResolveUsers = this.resolveUsers;

    this.incidentService.editIncident(this.dataModel).subscribe(
      response => {
        this.appNotificationService.success('Incident Updated Successfully!');
        this.router.navigate(['incident']);
      },
      (error) => {
        //Error Handling
        this.apiErrorService.handleError(error, () => {
          this.appNotificationService.error("Incident Failed to Update!");
        });
      });

  }

  //Get Incident Data For Selected Incident
  getIncidentData() {
    this.incidentService.getIncidentData(this.id).subscribe(payloadResponse => {
      this.incidentEditForm.get('incidentId').setValue(payloadResponse['incidentId']);
      this.incidentEditForm.get('status').setValue(payloadResponse['status']);
      this.customstatus = payloadResponse['status'];
      this.incidentEditForm.get('category').setValue(payloadResponse['categoriesMstrId']);
      this.incidentEditForm.get('description').setValue(payloadResponse['description']);
      this.incidentEditForm.get('notes').setValue(payloadResponse['notes']);
      this.incidentEditForm.get('noOfProperties').setValue(payloadResponse['noOfPropsAffected']);
      this.incidentEditForm.get('noOfZones').setValue(payloadResponse['noOfZones']);
      this.incidentEditForm.get('noOfCells').setValue(payloadResponse['noOfCells']);
      this.incidentEditForm.get('noOfPropertiesRestoared').setValue(payloadResponse['noOfPropsRestored']);
    });
  }

  //Get Category List
  getCategoryList() {
    this.incidentService.getCategoryDataList().subscribe(CategoryListResponse => {
      for (let category of CategoryListResponse) {
        this.formCategories.push({
          id: category.id,
          name: category.category
        });
      }
    });
  }

  onSubmit() {
    // this.uploadbuttonvisible = true;

    //Status---1 (If you want to Complete the Incident)
    if (this.incidentEditForm.get('status').value == 1) {
      if (this.incidentEditForm.get('noOfProperties').value - this.incidentEditForm.get('noOfPropertiesRestoared').value > 0) {
        this.title = "Confirmation";
        this.modalfor = "confirmation";
        this.modalbody = this.incidentEditForm.get('noOfProperties').value - this.incidentEditForm.get('noOfPropertiesRestoared').value + " propertie(s) are not yet restored. Still do you wish to complete the incident?";
        this.opendialog.nativeElement.click();
      }
    }
    //Status---2 (If you want to Cancel the Incident)
    else if (this.incidentEditForm.get('status').value == 2) {
      this.title = "Confirmation";
      this.modalfor = "confirmation";
      this.modalbody = "Are you sure you want to cancel the incident?";
      this.opendialog.nativeElement.click();
    }
    else {
      //Status--0 (If you want to complete the incident)

      let flag: boolean;
      flag = true;
      this.dataModel.MPRNs = [];
      this.Properties = [];
      this.dataModel.Id = this.id;
      this.dataModel.CategoriesMstrId = this.incidentEditForm.value.category;
      this.dataModel.Description = this.incidentEditForm.value.description;
      this.dataModel.Notes = this.incidentEditForm.value.notes;
      this.dataModel.Status = this.incidentEditForm.value.status;
      var priorityCustomer: boolean;
      console.log('(this.mprnFileinJSONFormat');
      console.log(this.mprnFileinJSONFormat);
      if (this.mprnFileinJSONFormat != null && this.mprnFileinJSONFormat) {
        this.mprnFileinJSONFormat.forEach(element => {
          const incidentPropertiesRequestObject = new IncidentPropertiesRequest();
          //Json File send to data model
          if (element.MPRN != null && element.MPRN && element.MPRN != "") {
            incidentPropertiesRequestObject.MPRN = element.MPRN;
            incidentPropertiesRequestObject.BuildingName = element.BuildingName;
            incidentPropertiesRequestObject.SubBuildingName = element.SubBuildingName;
            incidentPropertiesRequestObject.BuildingNumber = element.BuildingNumber;
            incidentPropertiesRequestObject.MCBuildingName = element.MCBuildingName;
            incidentPropertiesRequestObject.MCSubBuildingName = element.MCSubBuildingName;
            incidentPropertiesRequestObject.PrincipalStreet = element.PrincipalStreet;
            incidentPropertiesRequestObject.DependentStreet = element.DependentStreet;
            incidentPropertiesRequestObject.PostTown = element.PostTown;
            incidentPropertiesRequestObject.LocalityName = element.LocalityName;
            incidentPropertiesRequestObject.DependentLocality = element.DependentLocality;
            incidentPropertiesRequestObject.Country = element.Country;
            incidentPropertiesRequestObject.Postcode = element.PostcodeOutcode + " " + element.PostcodeIncode;
            if (element.PriorityCustomer == 'Y') {
              incidentPropertiesRequestObject.PriorityCustomer = true;
            }
            else {
              incidentPropertiesRequestObject.PriorityCustomer = false;
            }

            incidentPropertiesRequestObject.Zone = element.Zone;
            incidentPropertiesRequestObject.Cell = element.Cell;
            incidentPropertiesRequestObject.IncidentId = this.id;
            incidentPropertiesRequestObject.ZoneManagerName = element.ZoneController;
            incidentPropertiesRequestObject.CellManagerName = element.CellManager;
            this.Properties.push(incidentPropertiesRequestObject);
          }
          else {
            this.uploadbuttonvisible = false;
            this.appNotificationService.error("MPRN should Not be blank!");
            flag = false;
          }
        });

      }
      this.dataModel.MPRNs = this.Properties;
      if (flag) {
        this.uploadbuttonvisible = true;
        this.incidentService.editIncident(this.dataModel).subscribe(
          response => {
            this.uploadbuttonvisible = false;
            this.router.navigate(['incident']).then(this.appNotificationService.success('Incident Updated Successfully!'));
          },
          (error) => {
            //Error Handling
            this.uploadbuttonvisible = false;
            this.apiErrorService.handleError(error, () => {
              const x = error.error.message;
              const y = error.error.users;
              //If Same User Name(Zone Manager/Cell Manager) Found with Different Email Id
              if (error.error.message == "DUPLICATE_USERS_FOUND") {
                let i: number;
                this.emailIds = [];
                // get unique user names form received arrray.
                const duplicateusers = error.error.users.map(item => item.Name).filter((value, index, self) => self.indexOf(value) === index);

                this.duplicateIncidentRec = [];
                // make a new datastructure for rendering ui like saparete radio button gruop with user name.
                for (const user of duplicateusers) {
                  const duplicateuserobj = new DuplicateIncidentRec();
                  duplicateuserobj.username = user;
                  duplicateuserobj.emaillist = [];
                  const emailarray = error.error.users.filter(item => item.Name === user);
                  for (const email of emailarray) {
                    duplicateuserobj.emaillist.push(email);
                  }
                  this.duplicateIncidentRec.push(duplicateuserobj);
                }//end of creation of custom data structure for ui render.              

                // for (i = 0; i < error.error.users.length; i++) {
                //   this.emailIds.push(error.error.users[i].Email);
                //   this.selectedName = error.error.users[i].Name;
                // }

                this.title = "Duplicate Records";
                this.modalfor = "duplicaterecords";
                this.modalbody = "Resolve Duplicate Record by Selecting one of Email ID";
                this.opendialog.nativeElement.click();
              }
              //If User(Zone Manager/Cell Manager) Not Exist in system
              if (error.error.message == "USER_DOES_NOT_EXISTS") {
                let i: number;
                for (i = 0; i < error.error.users.length; i++) {
                  if (this.users.filter != error.error.users) {
                    this.users.push(error.error.users[i]);
                  }
                }
                this.title = "User Not Exist";
                this.modalfor = "userNotExist";
                this.modalbody = this.users + " User Not Exist in the System";
                this.opendialog.nativeElement.click();
              }
              if (error.error.message == "DUPLICATE_MPRNS_FOUND_IN_REQ") {
                this.appNotificationService.error("Duplicate MPRN Found!");
              }
            });
          });
      }
    }
  }

  //If we change the status SelectChangeHandler Getting Called
  selectChangeHandler() {

    //Status--1 (Complete)
    if (this.incidentEditForm.get('status').value == 1) {
      if (this.incidentEditForm.get('noOfProperties').value - this.incidentEditForm.get('noOfPropertiesRestoared').value > 0) {
        this.title = "Confirmation";
        this.modalfor = "confirmation";
        this.modalbody = this.incidentEditForm.get('noOfProperties').value - this.incidentEditForm.get('noOfPropertiesRestoared').value + " propertie(s) are not yet restored. Still do you wish to complete the incident?";
        this.opendialog.nativeElement.click();
      }
    }
    //Status--1 (Cancel)
    if (this.incidentEditForm.get('status').value == 2) {
      this.title = "Confirmation";
      this.modalfor = "confirmation";
      this.modalbody = "Are you sure you want to cancel the incident?";
      this.opendialog.nativeElement.click();
    }
  }

  yes() {
    this.dataModel.MPRNs = [];
    this.Properties = [];
    this.dataModel.Id = this.id;
    this.dataModel.CategoriesMstrId = this.incidentEditForm.value.category;
    this.dataModel.Description = this.incidentEditForm.value.description;
    this.dataModel.Notes = this.incidentEditForm.value.notes;
    this.dataModel.Status = this.incidentEditForm.value.status;
    var priorityCustomer: boolean;
    if (this.mprnFileinJSONFormat != null && this.mprnFileinJSONFormat) {
      this.mprnFileinJSONFormat.forEach(element => {
        const incidentPropertiesRequestObject = new IncidentPropertiesRequest();

        //Json File send to data model
        if (element.MPRN != null && element.MPRN && element.MPRN != "") {
          incidentPropertiesRequestObject.MPRN = element.MPRN;
          incidentPropertiesRequestObject.BuildingName = element.BuildingName;
          incidentPropertiesRequestObject.SubBuildingName = element.SubBuildingName;
          incidentPropertiesRequestObject.BuildingNumber = element.BuildingNumber;
          incidentPropertiesRequestObject.MCBuildingName = element.MCBuildingName;
          incidentPropertiesRequestObject.MCSubBuildingName = element.MCSubBuildingName;
          incidentPropertiesRequestObject.PrincipalStreet = element.PrincipalStreet;
          incidentPropertiesRequestObject.DependentStreet = element.DependentStreet;
          incidentPropertiesRequestObject.PostTown = element.PostTown;
          incidentPropertiesRequestObject.LocalityName = element.LocalityName;
          incidentPropertiesRequestObject.DependentLocality = element.DependentLocality;
          incidentPropertiesRequestObject.Country = element.Country;
          incidentPropertiesRequestObject.Postcode = element.PostcodeOutcode + " " + element.PostcodeIncode;
          if (element.PriorityCustomer == 'Y') {
            incidentPropertiesRequestObject.PriorityCustomer = true;
          }
          else {
            incidentPropertiesRequestObject.PriorityCustomer = false;
          }
          incidentPropertiesRequestObject.Zone = element.Zone;
          incidentPropertiesRequestObject.Cell = element.Cell;
          incidentPropertiesRequestObject.IncidentId = this.id;
          incidentPropertiesRequestObject.ZoneManagerName = element.ZoneManager;
          incidentPropertiesRequestObject.CellManagerName = element.cellManger;
          this.Properties.push(incidentPropertiesRequestObject);
        }
      });
    }
    this.dataModel.MPRNs = this.Properties;
    this.uploadbuttonvisible = true;
    this.incidentService.editIncident(this.dataModel).subscribe(
      response => {
        this.router.navigate(['incident']).then(this.appNotificationService.success('Incident Updated Successfully!'));
      },
      (error) => {
        //Error Handling
        this.apiErrorService.handleError(error, () => {
          const x = error.error.message;
          const y = error.error.users;
          //Duplicate User Found
          if (error.error.message == "DUPLICATE_USERS_FOUND") {
            let i: number;
            for (i = 0; i < error.error.users.length; i++) {
              this.emailIds.push(error.error.users[i].Email);
              this.selectedName = error.error.users[i].Name;
            }
            this.title = "Duplicate Records";
            this.modalfor = "duplicaterecords";
            this.modalbody = "Resolve Duplicate Record by Selecting one of Email ID";
            this.opendialog.nativeElement.click();
          }
          //If User not Exist in the System.
          if (error.error.message == "USER_DOES_NOT_EXISTS") {
            let i: number;
            for (i = 0; i < error.error.users.length; i++) {
              if (this.users.filter != error.error.users) {
                this.users.push(error.error.users[i]);
              }
            }
            this.title = "User Not Exist";
            this.modalfor = "userNotExist";
            this.modalbody = this.users + " User Not Exist in the System";
            this.opendialog.nativeElement.click();
          }
          if (error.error.message == "DUPLICATE_MPRNS_FOUND_IN_REQ") {
            this.appNotificationService.error("Duplicate MPRN Found!");
          }
        });
      });
  }

  //If You Dont want to cancel/Complete Incident
  no() {
    this.incidentEditForm.get('status').setValue(0);
  }

  // uploadFileClick func
  uploadFileButtonClick() {
    var resetForm = <HTMLFormElement>document.getElementById('fileInput');
    resetForm.value = null;
    this.fileInput.nativeElement.click();
  }

}
