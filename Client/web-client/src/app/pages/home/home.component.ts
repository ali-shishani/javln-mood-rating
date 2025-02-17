import { Component, OnInit } from '@angular/core';
import { GetMoodRatingOptionsResponse, MoodRatingOptionCode, MoodRatingOption, } from '../../core/models/mood.rating.models';
import { MoodRatingService } from './mood-rating.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  isLoading: boolean = false;
  allRatingOptions: MoodRatingOption[] = [];

constructor(
    private moodRatingService: MoodRatingService,
  ) { 

  }

  ngOnInit(): void {
    
    this.initialise();
  }

  initialise(): void{
    void this.moodRatingService.getMoodRatingOptions().subscribe((data: GetMoodRatingOptionsResponse) => {
      this.allRatingOptions = data.moodRatingOptions;
    });
  }
}
