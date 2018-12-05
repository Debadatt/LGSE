import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignMprnComponent } from './assign-mprn.component';

describe('AssignMprnComponent', () => {
  let component: AssignMprnComponent;
  let fixture: ComponentFixture<AssignMprnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignMprnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignMprnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
