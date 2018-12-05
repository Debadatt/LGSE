import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CellLevelAverageTimeTakenComponent } from './cell-level-average-time-taken.component';

describe('CellLevelAverageTimeTakenComponent', () => {
  let component: CellLevelAverageTimeTakenComponent;
  let fixture: ComponentFixture<CellLevelAverageTimeTakenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CellLevelAverageTimeTakenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CellLevelAverageTimeTakenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
