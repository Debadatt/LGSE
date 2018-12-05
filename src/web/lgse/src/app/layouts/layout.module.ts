import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MaterialModules } from '../modules/material-modules';
import { AppContainerComponent } from './app-layout/app-container/app-container.component';
import { ContentComponent } from './app-layout/content/content.component';
import { FooterComponent } from './app-layout/footer/footer.component';
import { TopHeaderComponent } from './app-layout/top-header/top-header.component';
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { SidebarComponent } from './app-layout/sidebar/sidebar.component';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MaterialModules
  ],
  declarations: [
    AppContainerComponent,
    ContentComponent,
    FooterComponent,
    TopHeaderComponent,
    AuthLayoutComponent,
    SidebarComponent,
  ],
})
export class LayoutModule { }
