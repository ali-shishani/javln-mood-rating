
import { Error } from "./error.models";

export enum MoodRatingOption {
    Good = 1
}

export interface GetMoodRatingOptionsResponse {
    ratingOptions: MoodRatingOption[];
    errors: Error[],
}

export interface RecordMoodRatingRequest {
    rating: MoodRatingOption;
}

export interface RecordMoodRatingResponse {
    isSuccessful: boolean;
    rating: MoodRatingOption;
    errors: Error[],
}

