import { Component, OnInit, Input } from '@angular/core';
import { MatExpansionPanel } from '@angular/material/expansion';
import { Subscription } from 'rxjs';
import { PassdataService } from 'src/app/services/passdata.service';
import { CustomSearchDataMain, CustomSearchData, SearchFilter } from 'src/app/models/search/custom-search-data';
import { FormControl, Validators, FormBuilder, FormGroup, NgForm } from '@angular/forms';
import { AppNotificationService } from 'src/app/services/notification/app-notification.service';
export interface Food {
  value: string;
  viewValue: string;
}
@Component({
  selector: 'app-custom-search',
  templateUrl: './custom-search.component.html',
  styleUrls: ['./custom-search.component.css']
})
export class CustomSearchComponent implements OnInit {
  @Input() customsearchdata: CustomSearchDataMain;
  closesearchvalue;
  searchfromgroup: FormGroup;
  serachsubscriptions: Subscription;
  customSearchData: CustomSearchData[] = [];
  searchFilter: SearchFilter[] = [];
  testarray: any[] = [];
  searchkey1: CustomSearchData;
  searchkey2: CustomSearchData;
  searchvalue1: any;
  searchvalue2: any;
  // process variables
  constructor(private passdataservice: PassdataService,
    private appnotificationservice: AppNotificationService) {
    console.log('focous received');
  }

  ngOnInit() {
    //this.createControls();
    this.testarray.push(2, 2);
    console.log('received obejct');
    console.log(this.customsearchdata);
    // assigned  received data to draopdown.
    if (this.customsearchdata && this.customsearchdata !== null) {
      this.customSearchData = this.customsearchdata.fieldlist;
      this.setSelected();
    }
    // end
    this.closesearchvalue = true;
    this.serachsubscriptions = this.passdataservice.openseachsubjects.subscribe((value) => {
      this.closesearchvalue = value;
      console.log('closesearchvalue', this.closesearchvalue);
    });

  } // end of nginit


  // code block for create form controls
  createControls(): void {
    this.searchfromgroup = new FormGroup({
      searchfieldcontrol0: new FormControl(),
      searchvaluecontrol0: new FormControl(),
      searchfieldcontrol1: new FormControl(),
      searchvaluecontrol1: new FormControl(),
    });
  }
  // end of code block

  expandPanel(matExpansionPanel: MatExpansionPanel, event: Event) {
    event.stopPropagation();
    matExpansionPanel.close();
  }
  closeSearch() {
    this.closesearchvalue = false;
  } //end 
  // code block for setting selected value in select box
  setSelected(): void {
    if (this.customsearchdata.selected.length > 0) {
      // for (let i = 0; i < this.customsearchdata.selected.length; i++) {
      //   const conttrolname = 'searchfieldcontrol' + i;
      //   this.searchfromgroup.get(conttrolname).setValue(this.customsearchdata.selected[i]);
      // }
      this.searchkey1 = this.customsearchdata.fieldlist.filter(filedlist => filedlist.searchkey === this.customsearchdata.selected[0])[0];
      this.searchkey2 = this.customsearchdata.fieldlist.filter(filedlist => filedlist.searchkey === this.customsearchdata.selected[1])[0];

    }
  }// end of set selected.

  // code block for passing search values to parent component
  passSearchkeysValues(): void {
    this.searchFilter = [];
    const fsearchke1 = this.searchkey1;
    const fsearchvalue1 = this.searchvalue1;
    const fsearchkey2 = this.searchkey2;
    const fsearchvalue2 = this.searchvalue2;
    if ((fsearchvalue1 != undefined && fsearchvalue1 != '') && (fsearchvalue2 != undefined && fsearchvalue2 != '')) {
      console.log('two values selected');
      if (fsearchke1 == fsearchkey2) {
        this.appnotificationservice.error('Two search key should not be same !!!');
        return;
      }
    }

    if (fsearchvalue1 != undefined && fsearchvalue1 != '') {
      this.searchFilter.push({ searchkey: fsearchke1.searchkey, searchvalue: fsearchvalue1 });
    }
    console.log(fsearchvalue2, fsearchvalue1);
    if (fsearchvalue2 != undefined && fsearchvalue2 != '') {
      this.searchFilter.push({ searchkey: fsearchkey2.searchkey, searchvalue: fsearchvalue2 });
    }
    console.log('filter value');
    console.log(this.searchFilter);
    // if (this.searchFilter.length > 0) {
    this.passdataservice.passSearchValues(this.searchFilter);
    //}
  }
  //end of fucntion.
  // cleanup 
  ngOnDestroy() {
    if (this.serachsubscriptions) {
      this.serachsubscriptions.unsubscribe();
    }
  }  // end of ngdistory
  test() {
    console.log('test');
    console.log('search key 1', this.searchkey1);
    console.log('search value 1', this.searchvalue1);

  }
  clearValue2(): void {
    this.searchvalue2 = '';
  }
  clearValue1(): void {
    this.searchvalue1 = '';
  }
  // code bllock for reseting ngmodels value
  resetData() {
    this.searchkey1 = null;
    this.searchvalue1 = null;
    this.searchkey2 = null;
    this.searchvalue2 = null;
    this.setSelected();
    this.passSearchkeysValues();
  }
  //end value
}// end of class
