import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentShowPropertiesComponent } from './incident-show-properties.component';

describe('IncidentShowPropertiesComponent', () => {
  let component: IncidentShowPropertiesComponent;
  let fixture: ComponentFixture<IncidentShowPropertiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentShowPropertiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentShowPropertiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
