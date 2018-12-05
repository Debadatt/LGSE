import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignResourceMprnComponent } from './assign-resource-mprn.component';

describe('AssignResourceMprnComponent', () => {
  let component: AssignResourceMprnComponent;
  let fixture: ComponentFixture<AssignResourceMprnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignResourceMprnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignResourceMprnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
