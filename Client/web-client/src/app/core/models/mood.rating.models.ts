
import { Error } from "./error.models";

export enum MoodRatingOptionCode {
    NotGoodAtAll = 1,
    AbitMeh = 2,
    PrettyGood = 3,
    FeelingGreat = 4,
}

export interface MoodRatingOption {
    code: MoodRatingOptionCode;
    displayName: string;
}

export interface GetMoodRatingOptionsResponse {
    moodRatingOptions: MoodRatingOption[];
    errors: Error[];
}

export interface RecordMoodRatingRequest {
    rating: MoodRatingOptionCode;
}

export interface RecordMoodRatingResponse {
    isSuccessful: boolean;
    rating: MoodRatingOption;
    errors: Error[];
}

