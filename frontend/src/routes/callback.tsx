import { createFileRoute, useNavigate, useRouter } from '@tanstack/react-router'
import { useAuth0Context } from '../auth/Auth0'
import { useEffect } from 'react'
import CircularProgress from '@mui/material/CircularProgress'
import Box from '@mui/material/Box'
import Typography from '@mui/material/Typography'

export const Route = createFileRoute('/callback')({
  component: CallbackComponent,
})

function CallbackComponent() {
  const { isLoading, isAuthenticated } = useAuth0Context()
  const navigate = useNavigate()
      const router = useRouter()

  useEffect(() => {
    if (!isLoading && isAuthenticated) {
      // Auth0 strips the code/state params after exchange — navigate to dashboard.
      // If you need to restore a pre-login destination, store it in
      // sessionStorage before calling login() and read it back here.
    //   navigate({ to: '/dashboard', replace: true })
   router.invalidate().then(() => {
        const returnTo = sessionStorage.getItem('returnTo') ?? '/trivia/questions'
        sessionStorage.removeItem('returnTo')
        navigate({ to: returnTo, replace: true })
      })
    }
  }, [isLoading, isAuthenticated, navigate])

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100dvh', gap: 2 }}>
      <CircularProgress size={24} />
      <Typography variant="body2" color="text.secondary">Signing you in…</Typography>
    </Box>
  )
}