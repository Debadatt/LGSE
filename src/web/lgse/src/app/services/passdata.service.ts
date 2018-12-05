import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { SearchFilter } from 'src/app/models/search/custom-search-data';
import { MapData } from 'src/app/models/api/map/incident-id';
import { BackButtonPath } from 'src/app/models/ui/back-btutton';
import { GetActiveMPRnList } from 'src/app/models/api/resources/get-active-mprn-response';


@Injectable()
export class PassdataService {
  openseachsubjects: Subject<boolean> = new Subject<boolean>();
  customsearchvalue: Subject<SearchFilter[]> = new Subject<SearchFilter[]>();
  fullscreensubject: Subject<boolean> = new Subject<boolean>();
  fullscreendivsubject: Subject<boolean> = new Subject<boolean>();
  callfullscreen: Subject<boolean> = new Subject<boolean>();
  backpath: BackButtonPath<any>;
  ismapvisible: Subject<boolean> = new Subject<boolean>();
  assignedMPRN: boolean;
  // assignedmprn:
  bingMapdata: MapData<any>;
  userName: string;
  roleName: string;
  selectedmprnllist: GetActiveMPRnList[] = [];
  constructor() { }

  setOpenSearch(value: boolean) {
    this.openseachsubjects.next(value);
  }
  passSearchValues(value: SearchFilter[]): void {
    this.customsearchvalue.next(value);
  }
  setFullscreen(val) {
    this.fullscreensubject.next(val);
  }
  setFullscreenDiv(val) {
    this.fullscreendivsubject.next(val);
  }
  setFullWidth() {
    this.callfullscreen.next();
  }
  setMapvisible() {
    this.ismapvisible.next(true);
  }
}
