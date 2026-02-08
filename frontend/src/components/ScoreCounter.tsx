
// import { useAppSelector, useAppDispatch } from 'app/hooks'

// import { decrement, increment } from './counterSlice'
// import { useAppDispatch, useAppSelector } from '../hooks'
import  { Typography } from '@mui/material'
import {  useAppSelector } from '../Store'

export function ScoreCounter() {
  // The `state` arg is correctly typed as `RootState` already
  const count = useAppSelector((state) => state.counter.value)

  // omit rendering logic

    return (
        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          Score: {count}
        </Typography>
    )
}