import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { Component, OnInit, NgModule } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IncidentService } from '../../../services/incident.service';
import { IncidentAddRequest, IncidentPropertiesRequest } from '../../../models/api/incident.model';
import { ApiErrorService } from '../../../services/api-error.service';
import { AppNotificationService } from '../../../services/notification/app-notification.service';
import { IncidentEditComponent } from './incident-edit.component';

describe('IncidentEditComponent', () => {
  let component: IncidentEditComponent;
  let fixture: ComponentFixture<IncidentEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});









