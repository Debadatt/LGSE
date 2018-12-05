import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ZoneLevelAverageTimeTakanComponent } from './zone-level-average-time-takan.component';

describe('ZoneLevelAverageTimeTakanComponent', () => {
  let component: ZoneLevelAverageTimeTakanComponent;
  let fixture: ComponentFixture<ZoneLevelAverageTimeTakanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ZoneLevelAverageTimeTakanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ZoneLevelAverageTimeTakanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
