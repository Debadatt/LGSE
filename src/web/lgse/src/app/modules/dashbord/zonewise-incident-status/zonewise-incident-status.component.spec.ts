import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ZonewiseIncidentStatusComponent } from './zonewise-incident-status.component';

describe('ZonewiseIncidentStatusComponent', () => {
  let component: ZonewiseIncidentStatusComponent;
  let fixture: ComponentFixture<ZonewiseIncidentStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ZonewiseIncidentStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ZonewiseIncidentStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
