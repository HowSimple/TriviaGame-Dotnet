// import { EntityBase } from "./entityBase";

export interface TriviaQuestion  {
    id: string;
    questionDescription: string;
    questionTopic: string;
    wrongAnswers: string[];
    correctAnswer: string;
}