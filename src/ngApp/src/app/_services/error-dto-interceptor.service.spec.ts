import { TestBed } from '@angular/core/testing';

import { ErrorDtoInterceptorService } from './error-dto-interceptor.service';

describe('ErrorDtoInterceptorService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ErrorDtoInterceptorService = TestBed.get(ErrorDtoInterceptorService);
    expect(service).toBeTruthy();
  });
});
