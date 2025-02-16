
import { Error } from "./error.models";

export enum MoodRatingOption {
    NotGoodAtAll = 1,
    AbitMeh = 2,
    PrettyGood = 3,
    FeelingGreat = 4,
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

