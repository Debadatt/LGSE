import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserLevelAverageTimeTakenComponent } from './user-level-average-time-taken.component';

describe('UserLevelAverageTimeTakenComponent', () => {
  let component: UserLevelAverageTimeTakenComponent;
  let fixture: ComponentFixture<UserLevelAverageTimeTakenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserLevelAverageTimeTakenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserLevelAverageTimeTakenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
