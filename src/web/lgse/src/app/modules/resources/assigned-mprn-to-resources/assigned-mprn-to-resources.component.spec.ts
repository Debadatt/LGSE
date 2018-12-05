import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedMprnToResourcesComponent } from './assigned-mprn-to-resources.component';

describe('AssignedMprnToResourcesComponent', () => {
  let component: AssignedMprnToResourcesComponent;
  let fixture: ComponentFixture<AssignedMprnToResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignedMprnToResourcesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignedMprnToResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
