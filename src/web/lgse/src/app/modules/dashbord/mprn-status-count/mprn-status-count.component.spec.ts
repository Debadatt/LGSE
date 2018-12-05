import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MprnStatusCountComponent } from './mprn-status-count.component';

describe('MprnStatusCountComponent', () => {
  let component: MprnStatusCountComponent;
  let fixture: ComponentFixture<MprnStatusCountComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MprnStatusCountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MprnStatusCountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
