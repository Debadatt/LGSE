import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserWiseIncidentSummaryComponent } from './user-wise-incident-summary.component';

describe('UserWiseIncidentSummaryComponent', () => {
  let component: UserWiseIncidentSummaryComponent;
  let fixture: ComponentFixture<UserWiseIncidentSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserWiseIncidentSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserWiseIncidentSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
