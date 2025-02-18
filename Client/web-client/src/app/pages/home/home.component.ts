import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import {MatIconRegistry, MatIconModule} from '@angular/material/icon';
import { GetMoodRatingOptionsResponse, MoodRatingOptionCode, MoodRatingOption, } from '../../core/models/mood.rating.models';
import { MoodRatingService } from './mood-rating.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  isLoading: boolean = false;
  allRatingOptions: MoodRatingOption[] = [];

  selectedRating = 0;
  email = '';
  comment = '';

  feelingText = 'Select your mood!';
  stars = [
    {
      id: 1,
      icon: 'star',
      class: 'star-gray star-hover star'
    },
    {
      id: 2,
      icon: 'star',
      class: 'star-gray star-hover star'
    },
    {
      id: 3,
      icon: 'star',
      class: 'star-gray star-hover star'
    },
    {
      id: 4,
      icon: 'star',
      class: 'star-gray star-hover star'
    }
  ];

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

  isValid(): boolean{
    // prevent multiple selection
    if(this.selectedRating === 0 || !this.isValidEmail()){
      return false;
    }
    return true;
  }

  isValidEmail(): boolean{
    if(this.email && this.email.length > 0){
      true;
    }
    return false;
  }

  selectStar(value: number): void{
    // prevent multiple selection
    if(this.selectedRating === 0){
      this.stars.filter( (star) => {
        if ( star.id <= value){
          star.class = 'star-gold star';
        }else{
          star.class = 'star-gray star';
        }
        return star;
      });
    }

    this.selectedRating = value;
  }

  onMouseOver(value: number): void {
    if ( this.selectedRating === 0){
      switch(value)
      {
        case 1:
          this.feelingText = 'Not good at all';    
          break;
        case 2:
          this.feelingText = 'A bit “meh”';
          break;
        case 3:
          this.feelingText = 'Pretty good';
          break;
        case 4:
          this.feelingText = 'Feeling great';
          break;
        default:
          break;
      }
    }
  }

  onMouseLeave(value: number): void {
    if ( this.selectedRating === 0){
      this.feelingText = 'Select your mood!';
    }
  }
}
