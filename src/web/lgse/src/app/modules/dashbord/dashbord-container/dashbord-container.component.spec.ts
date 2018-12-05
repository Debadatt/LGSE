import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashbordContainerComponent } from './dashbord-container.component';

describe('DashbordContainerComponent', () => {
  let component: DashbordContainerComponent;
  let fixture: ComponentFixture<DashbordContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashbordContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashbordContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
