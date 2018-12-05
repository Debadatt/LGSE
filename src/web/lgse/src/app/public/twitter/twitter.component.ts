import { Component, OnInit } from '@angular/core';
import { DynamicWelcomeText } from '../../services/dynamicwelcometext.service';

@Component({
  selector: 'app-twitter',
  templateUrl: './twitter.component.html',
  styleUrls: ['./twitter.component.css']
})
export class TwitterComponent implements OnInit {

  welcomeText:string;
  constructor( private dynamicWelcomeTextService:DynamicWelcomeText) { }

  ngOnInit() {
    this.dynamicWelcomeTextService.getDynamicWelcomeText().subscribe(DynmaicWelcomeTextResponse => {
      console.log("dynamic text response",DynmaicWelcomeTextResponse);
     if(DynmaicWelcomeTextResponse!=null)
     {
      this.welcomeText=DynmaicWelcomeTextResponse['description'];
     }
    });
  }

}
