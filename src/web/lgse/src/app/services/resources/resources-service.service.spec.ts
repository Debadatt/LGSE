import { TestBed, inject } from '@angular/core/testing';

import { ResourcesServiceService } from './resources-service.service';

describe('ResourcesServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ResourcesServiceService]
    });
  });

  it('should be created', inject([ResourcesServiceService], (service: ResourcesServiceService) => {
    expect(service).toBeTruthy();
  }));
});
