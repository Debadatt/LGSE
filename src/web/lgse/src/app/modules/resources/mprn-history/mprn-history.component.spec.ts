import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MprnHistoryComponent } from './mprn-history.component';

describe('MprnHistoryComponent', () => {
  let component: MprnHistoryComponent;
  let fixture: ComponentFixture<MprnHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MprnHistoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MprnHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
