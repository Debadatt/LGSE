import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IncidentService } from '../../../services/incident.service';
import { IncidentAddRequest, IncidentPropertiesRequest, IncidentResolveUsersRequest } from '../../../models/api/incident.model';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { CsvHeareds } from 'src/app/app-common-constants';
import { DuplicateIncidentRec } from 'src/app/models/ui/incident/duplicate-incident-rec';

@Component({
  selector: 'app-incident-add',
  templateUrl: './incident-add.component.html',
  styleUrls: ['./incident-add.component.css']
})

export class IncidentAddComponent implements OnInit {

  //Variable Declaration
  incidentAddForm: FormGroup;
  propertyModel = new IncidentPropertiesRequest();
  resolveUser = new IncidentResolveUsersRequest();
  formCategories = [];
  emailIds = [];
  users = [];
  mprnFileinJSONFormat: any[];
  noOfCells: number;
  noOfProperties: number;
  noOfZones: number;
  selectedUser: string;
  selectedName: string;
  title: string;
  modalbody: string;
  modalfor: string;
  uploadbuttonvisible = false;
  isValid = true;
  dataModel = new IncidentAddRequest();
  Properties: IncidentPropertiesRequest[] = [];
  resolveUsers: IncidentResolveUsersRequest[] = [];
  duplicateIncidentRec: DuplicateIncidentRec[] = [];
  @ViewChild('fileInput') fileInput: ElementRef;
  @ViewChild('opendialog') opendialog: ElementRef;
  //End Declaration

  constructor(private router: Router,
    private incidentService: IncidentService,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService) { }

  ngOnInit() {
    this.incidentAddForm = new FormGroup({
      'incidentId': new FormControl(null),
      'description': new FormControl(null, [Validators.required]),
      'category': new FormControl(null, [Validators.required]),
      'notes': new FormControl(null),
      'uploadAffectedProperties': new FormControl(null),
      'noOfProperties': new FormControl(null),
      'noOfZones': new FormControl(null),
      'noOfCells': new FormControl(null)
    });
    this.getCategoryList();
  }

  //get categoty List
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

  //Conversion of CSV file to JSON Object
  private csvJSON(csv) {
    console.log('csv to json');
    // spliting file from
    var linesRead = csv.split("\r\n");
    // validation for blank csv
    if (linesRead.length === 0) {
      this.appNotificationService.error("You have entered invalid headers in CSV");
      return;
    }
    var result = [];
    // creating headers
    var headers = linesRead[0].split(",");
    let invalidheaders = false;
    // check for invalied headers.
    for (const header in headers) {
      console.log('header', headers[header]);
      const item = CsvHeareds.filter(commanheader => commanheader == headers[header])[0];
      if (!item) {
        invalidheaders = true;
        break;
      }
    }
    // if headers are invalied then throw errror.
    if (invalidheaders) {
      this.appNotificationService.error("invalid headers in csv");
      return;
    }
    const lines = [];
    // validation for balck line in csv.if blank line found in csv then remove it.
    for (let lineslen = 1; lineslen < linesRead.length; lineslen++) {
      if (linesRead[lineslen] == '') {
        break;
      }
      else {
        lines.push(linesRead[lineslen]);
      }
    }
    // validation check for blank csv upload
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
  }//  end of fucntion.

  public uploadCsvFile() {

    this.noOfProperties = 0;
    this.noOfZones = 0;
    this.noOfCells = 0;
    var file = (<HTMLInputElement>document.getElementById('fileInput')).files[0];
    if (file != undefined && file !== null) {
      if (file.type.includes("excel")) {
        const reader = new FileReader();
        reader.onload = () => {
          let text = reader.result;
          //convert text to json here
          this.mprnFileinJSONFormat = this.csvJSON(text);
          console.log('this.mprnFileinJSONFormat');
          console.log(this.mprnFileinJSONFormat);
          // if csv is valied then execute following code.
          if (this.mprnFileinJSONFormat) {
            this.calculateCSVSummary();
            // TODO: Set variable to zero
          }
          // TODO: call only if the conversion to JSON is successfull.
          //Calucalte No. Of Properties, Zone and cell from CSV File

        };
        reader.readAsText(file);
      } else {
        this.appNotificationService.error("Upload CSV File");
      }
    }

  }

  //Calculate Count for No Of Properties,Zone and Cell
  calculateCSVSummary() {
    this.incidentAddForm.get('noOfProperties').setValue(this.mprnFileinJSONFormat.length);
    this.incidentAddForm.get('noOfZones').setValue(this.mprnFileinJSONFormat.map(item => item.Zone)
      .filter((value, index, self) => self.indexOf(value) === index).length);
    this.incidentAddForm.get('noOfCells').setValue(this.mprnFileinJSONFormat.map(item => item.Cell)
      .filter((value, index, self) => self.indexOf(value) === index).length);
  }

  //Check Duplicate Record
  checkDuplicateRecord() {
    //If User Name(Zone Manager/Cell Manager) Is Duplicate(same) and Email Id Is Different

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

    var selectedRecord: string;
    selectedRecord = this.selectedUser;
    // this.dataModel.ResolveUsers = [];
    // const incidentResolveUsersRequestObject = new IncidentResolveUsersRequest();
    // incidentResolveUsersRequestObject.Email = this.selectedUser;
    // incidentResolveUsersRequestObject.Name = this.selectedName;
    // this.resolveUsers.push(incidentResolveUsersRequestObject);
    // this.dataModel.ResolveUsers = this.resolveUsers;
    this.dataModel.ResolveUsers = [];
    // pushing selected data to in API input
    for (let userrec of this.duplicateIncidentRec) {
      if (userrec.selecteditem) {
        this.resolveUsers.push(userrec.selecteditem);
      }
    }
    this.dataModel.ResolveUsers = this.resolveUsers;

    this.incidentService.addIncident(this.dataModel).subscribe(
      response => {
        this.appNotificationService.success('Incident Created Successfully!');
        this.router.navigate(['incident']);
      },
      (error) => {
        //Error Handling
        this.apiErrorService.handleError(error, () => {
          this.appNotificationService.error("Incident Failed to Create!");
        });
      });

  }

  onSubmit() {
    if (this.mprnFileinJSONFormat != null && this.mprnFileinJSONFormat) {
      this.uploadbuttonvisible = true;
      this.dataModel.MPRNs = [];
      this.Properties = [];
      //Added data into data model
      this.dataModel.CategoriesMstrId = this.incidentAddForm.value.category;
      this.dataModel.Description = this.incidentAddForm.value.description;
      this.dataModel.Notes = this.incidentAddForm.value.notes;
      this.Properties = this.propertiesValidate();
      this.dataModel.MPRNs = this.Properties;
      if (this.isValid) {
        this.incidentService.addIncident(this.dataModel).subscribe(
          response => {
            this.uploadbuttonvisible = false;
            this.router.navigate(['incident']).then(this.appNotificationService.success('Incident Created Successfully!'));
          },
          (error) => {
            //Handling Error
            this.uploadbuttonvisible = false;
            this.apiErrorService.handleError(error, () => {
              const x = error.error.message;
              const y = error.error.users;
              //If Same User Name(Zone manager/cellManager) and Different Email ID 
              if (error.error.message == "DUPLICATE_USERS_FOUND") {
                let i: number;
                this.emailIds = [];
                // if duplicate emails found 
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
                // }// end of loop.
                this.uploadbuttonvisible = false;
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
                this.uploadbuttonvisible = false;
                this.opendialog.nativeElement.click();
              }
              //If Already Incident is in 'InProgress' Second Incident we can not create untill First one is completed/Cancelled
              if (error.error.message == "INPROGRESS_INCIDENT_EXISTS") {
                this.appNotificationService.error("Incident Is In Progress!");
                this.uploadbuttonvisible = false;
              }
              //Duplicate MPRN should not be added for Same Incident
              if (error.error.message == "DUPLICATE_MPRNS_FOUND_IN_REQ") {
                this.appNotificationService.error("Duplicate MPRN Found!");
                this.uploadbuttonvisible = false;
              }
            });
          });
      }
    } else {
      this.appNotificationService.error("CSV data not in proper format");
      return;
    }
  } // end of function

  propertiesValidate() {
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
          incidentPropertiesRequestObject.IncidentId = element.IncidentId;
          incidentPropertiesRequestObject.ZoneManagerName = element.ZoneController;
          incidentPropertiesRequestObject.CellManagerName = element.CellManager;
          this.Properties.push(incidentPropertiesRequestObject);
        }
        else {
          this.isValid = false;
          this.uploadbuttonvisible = false;
          this.appNotificationService.error("CSV File Not in Proper Format!");
        }
      });
    }
    return this.Properties;
  }

  // uploadFileClick function
  uploadFileButtonClick() {
    // reseting values of fileinput.
    var resetForm = <HTMLFormElement>document.getElementById('fileInput');
    resetForm.value = null;
    this.fileInput.nativeElement.click();
  }
  // end
}// end of class








