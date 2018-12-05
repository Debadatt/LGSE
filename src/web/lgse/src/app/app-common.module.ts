import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { ModuleWithProviders, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AuthGuard } from './auth-guard';
import { MaterialModules } from './modules/material-modules';
import { TranslatePipe } from './pipes/translate/translate.pipe';
import { TranslateService } from './pipes/translate/translate.service';
import { AppNotificationService } from './services/notification/app-notification.service';
import { CustomSearchComponent } from 'src/app/custom/custom-search/custom-search.component';
import { PassdataService } from 'src/app/services/passdata.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    ReactiveFormsModule,
    MaterialModules,
    FormsModule
  ],
  declarations: [
    TranslatePipe,
    CustomSearchComponent
  ],
  exports: [
    TranslatePipe,
    MaterialModules,
    ReactiveFormsModule,
    CustomSearchComponent,
    FormsModule
  ]
})
export class AppCommonModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: AppCommonModule,
      providers: [
        DatePipe,
        DecimalPipe,
        AuthGuard,
        TranslatePipe,
        TranslateService,
        PassdataService,
        AppNotificationService
      ]
    };
  }
}
