import { TestBed, inject } from '@angular/core/testing';

import { DashbordService } from './dashbord.service';

describe('DashbordService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DashbordService]
    });
  });

  it('should be created', inject([DashbordService], (service: DashbordService) => {
    expect(service).toBeTruthy();
  }));
});
