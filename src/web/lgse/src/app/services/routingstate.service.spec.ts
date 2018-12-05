import { TestBed, inject } from '@angular/core/testing';

import { RoutingstateService } from './routingstate.service';

describe('RoutingstateService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RoutingstateService]
    });
  });

  it('should be created', inject([RoutingstateService], (service: RoutingstateService) => {
    expect(service).toBeTruthy();
  }));
});
