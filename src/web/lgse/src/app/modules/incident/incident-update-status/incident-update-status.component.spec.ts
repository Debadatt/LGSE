import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentUpdateStatusComponent } from './incident-update-status.component';

describe('IncidentUpdateStatusComponent', () => {
  let component: IncidentUpdateStatusComponent;
  let fixture: ComponentFixture<IncidentUpdateStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentUpdateStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentUpdateStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
