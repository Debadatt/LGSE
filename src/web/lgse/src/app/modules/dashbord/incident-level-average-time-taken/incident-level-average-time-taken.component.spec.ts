import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentLevelAverageTimeTakenComponent } from './incident-level-average-time-taken.component';

describe('IncidentLevelAverageTimeTakenComponent', () => {
  let component: IncidentLevelAverageTimeTakenComponent;
  let fixture: ComponentFixture<IncidentLevelAverageTimeTakenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentLevelAverageTimeTakenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentLevelAverageTimeTakenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
