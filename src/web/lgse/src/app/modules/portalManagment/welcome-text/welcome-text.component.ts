import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DynamicWelcomeText } from '../../../services/dynamicwelcometext.service';
import { GetDynamicWelcomeTextResponse, WelcomeTextAddRequest } from '../../../models/api/dynamic.welcometext.model';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { FeatureNames } from 'src/app/app-common-constants';
import { LocalstorageService } from 'src/app/services/localstorage.service';
import { ApiErrorService } from '../../../services/api-error.service';

@Component({
  selector: 'app-welcome-text',
  templateUrl: './welcome-text.component.html',
  styleUrls: ['./welcome-text.component.css']
})
export class WelcomeTextComponent implements OnInit {

  //Variable Declaration
  welcomeTextForm: FormGroup;
  feature = FeatureNames;
  id: string;

  constructor(private router: Router,
    private dynamicWelcomeTextService: DynamicWelcomeText,
    private apiErrorService: ApiErrorService,
    private appNotificationService: AppNotificationService,
    public localstorageservice: LocalstorageService
  ) { }

  ngOnInit() {
    this.welcomeTextForm = new FormGroup({
      'isDefaultText': new FormControl(null),
      'welcomeText': new FormControl(null),
    });
    this.getDynamicWelcomeText();
  }

  getDynamicWelcomeText() {
    this.dynamicWelcomeTextService.getWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      if (DynmaicWelcomeTextResponse != null) {
        this.welcomeTextForm.get('welcomeText').setValue(DynmaicWelcomeTextResponse['description']);
        this.welcomeTextForm.get('isDefaultText').setValue(DynmaicWelcomeTextResponse['isActive']);
        this.id = DynmaicWelcomeTextResponse['id'];
      }
    });
  }
  onSubmit() {
    var dataModel = new WelcomeTextAddRequest();

    //Welcome text should mandatory when default check box is unchecked
    if (this.welcomeTextForm.value.isDefaultText == false && this.welcomeTextForm.value.welcomeText.trim().length == 0) {
      this.appNotificationService.error("Please Add Welcome Text!")
    }
    else {
      //Added data into data model
      dataModel.description = this.welcomeTextForm.value.welcomeText.trim();
      dataModel.isActive = this.welcomeTextForm.value.isDefaultText;

      //Call API for Update Welcome Text
      this.dynamicWelcomeTextService.updateWelcomeText(dataModel, this.id).subscribe(
        response => {
          this.router.navigate(['/portalManagment/userList']).then(this.appNotificationService.success('Welcome Text Updated Successfully!'));
        },
        (error) => {
          this.apiErrorService.handleError(error, () => {
            this.router.navigate(['/portalManagment/userList']).then(this.appNotificationService.error("Failed To Update Welcome Text!"));
          });
        });
    }
  }
}







