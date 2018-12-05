import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CellwiseIncidentStatusComponent } from './cellwise-incident-status.component';

describe('CellwiseIncidentStatusComponent', () => {
  let component: CellwiseIncidentStatusComponent;
  let fixture: ComponentFixture<CellwiseIncidentStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CellwiseIncidentStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CellwiseIncidentStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
