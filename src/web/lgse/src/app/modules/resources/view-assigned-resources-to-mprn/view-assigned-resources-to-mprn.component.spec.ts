import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAssignedResourcesToMprnComponent } from './view-assigned-resources-to-mprn.component';

describe('ViewAssignedResourcesToMprnComponent', () => {
  let component: ViewAssignedResourcesToMprnComponent;
  let fixture: ComponentFixture<ViewAssignedResourcesToMprnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAssignedResourcesToMprnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAssignedResourcesToMprnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
