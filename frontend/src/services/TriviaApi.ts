// Need to use the React-specific entry point to import createApi
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import type { TriviaQuestion } from '../models/TriviaQuestion'
// import type { Pokemon } from './types'

// Define a service using a base URL and expected endpoints
export const triviaApi = createApi({
  reducerPath: 'triviaApi',
  baseQuery: fetchBaseQuery({ baseUrl: 'http://localhost:8000/api/' }),
  endpoints: (builder) => ({
    // getPokemonByName: builder.query<TriviaQuestion, string>({
    //   query: (name) => `TriviaQuestions/${name}`,
    // }),
     getQuestions: builder.query<TriviaQuestion[], void>({
      query: () => `TriviaQuestions/`,
      transformResponse: (response: TriviaQuestion[]) =>{
        response.map(reponse => {
          reponse.wrongAnswers.push(reponse.correctAnswer);
          // = JSON.parse(reponse.wrongAnswers as unknown as string) as string[];
        })
        // reponse.Add()
        
        return response;
      },
    }),
  }),
})

// Export hooks for usage in functional components, which are
// auto-generated based on the defined endpoints
export const { useGetQuestionsQuery } = triviaApi