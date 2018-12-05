import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignedResourcesComponent } from './assigned-resources.component';

describe('AssignedResourcesComponent', () => {
  let component: AssignedResourcesComponent;
  let fixture: ComponentFixture<AssignedResourcesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignedResourcesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignedResourcesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
