import { TestBed, inject } from '@angular/core/testing';

import { PassdataService } from './passdata.service';

describe('PassdataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PassdataService]
    });
  });

  it('should be created', inject([PassdataService], (service: PassdataService) => {
    expect(service).toBeTruthy();
  }));
});
