import { TestBed } from '@angular/core/testing';

import { MoodRatingService } from './mood-rating.service';

describe('MoodRatingService', () => {
  let service: MoodRatingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MoodRatingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
