import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EngineeringCapacityReportComponent } from './engineering-capacity-report.component';

describe('EngineeringCapacityReportComponent', () => {
  let component: EngineeringCapacityReportComponent;
  let fixture: ComponentFixture<EngineeringCapacityReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EngineeringCapacityReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EngineeringCapacityReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
