import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import type { TriviaQuestion } from '../models/TriviaQuestion'

export interface HighScore {
  id: string
  userId: string
  userName?: string
  score: number
  created: string
  lastModified: string
}

export interface SaveHighScoreRequest {
  score: number
  userName?: string
}

// Holds the token getter so RTK Query can call it per-request.
// Set this once at app startup via setTokenGetter().
let _getToken: (() => Promise<string>) | null = null

export function setTokenGetter(fn: () => Promise<string>) {
  _getToken = fn
}

export const triviaApi = createApi({
  reducerPath: 'triviaApi',
  baseQuery: fetchBaseQuery({
    baseUrl: 'http://localhost:8000/api/',
    prepareHeaders: async (headers) => {
      if (_getToken) {
        try {
          const token = await _getToken()
          headers.set('Authorization', `Bearer ${token}`)
        } catch {
          // Not authenticated — request will proceed without a token.
          // Protected endpoints will return 401 as expected.
        }
      }
      return headers
    },
  }),
  tagTypes: ['HighScore'],
  endpoints: (builder) => ({
    getQuestions: builder.query<TriviaQuestion[], void>({
      query: () => 'TriviaQuestions/',
      transformResponse: (response: TriviaQuestion[]) => {
        response.forEach((q) => {
          q.wrongAnswers.push(q.correctAnswer)
        })
        return response
      },
    }),

    saveHighScore: builder.mutation<HighScore, SaveHighScoreRequest>({
      query: (body) => ({
        url: 'HighScores',
        method: 'POST',
        body,
      }),
      invalidatesTags: ['HighScore'],
    }),

    getMyScore: builder.query<HighScore, void>({
      query: () => 'HighScores/me',
      providesTags: ['HighScore'],
    }),

    getLeaderboard: builder.query<HighScore[], void>({
      query: () => 'HighScores/leaderboard',
    }),
  }),
})

export const {
  useGetQuestionsQuery,
  useSaveHighScoreMutation,
  useGetMyScoreQuery,
  useGetLeaderboardQuery,
} = triviaApi