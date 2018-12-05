import { TestBed, inject } from '@angular/core/testing';

import { LoclstorageService } from './loclstorage.service';

describe('LoclstorageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoclstorageService]
    });
  });

  it('should be created', inject([LoclstorageService], (service: LoclstorageService) => {
    expect(service).toBeTruthy();
  }));
});
