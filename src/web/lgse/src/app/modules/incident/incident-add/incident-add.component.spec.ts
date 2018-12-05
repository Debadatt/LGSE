import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IncidentAddComponent } from './incident-add.component';

describe('IncidentAddComponent', () => {
  let component: IncidentAddComponent;
  let fixture: ComponentFixture<IncidentAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IncidentAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IncidentAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
