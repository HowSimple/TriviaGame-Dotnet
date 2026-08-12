import { createFileRoute } from '@tanstack/react-router'
import { redirect } from '@tanstack/react-router'
import { z } from 'zod'

import LoginButton from '../components/LoginButton'

const fallback = '/dashboard' as const

export const Route = createFileRoute('/login')({
  validateSearch: z.object({
    redirect: z.string().optional().catch(''),
  }),
  beforeLoad: ({ context, search }) => {
    if (context.auth?.isAuthenticated) {
      throw redirect({ to: search.redirect || fallback })
    }
  },
  component: LoginComponent,
})

function LoginComponent() {
  const search = Route.useSearch()

  return (
    <div>
      {search.redirect && (
        <p>You need to log in to access this page.</p>
      )}
      <LoginButton />
    </div>
  )
}