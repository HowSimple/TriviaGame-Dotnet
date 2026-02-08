import * as React from 'react'
import { useRouter, useRouterState } from '@tanstack/react-router'

import { sleep } from '../utils'
import { useAuth0Context } from '../auth/Auth0'

interface LoginProps {
  redirectTo?: string
}

export function Login({ redirectTo = '/dashboard' }: LoginProps) {
  const auth = useAuth0Context()
  const router = useRouter()
  const isLoading = useRouterState({ select: (s) => s.isLoading })
  const [isSubmitting, setIsSubmitting] = React.useState(false)
    const handleLogin = async () => {
    await auth.login()
  }
  const onFormSubmit = async (evt: React.FormEvent<HTMLFormElement>) => {
    setIsSubmitting(true)
    try {
      evt.preventDefault()
      const data = new FormData(evt.currentTarget)
      const username = data.get('username')
      const password = data.get('password')

      if (!username || !password) return
      const usernameStr = username.toString()
      await auth.login(usernameStr  , password.toString())
      await router.invalidate()

      // This is just a hack being used to wait for the auth state to update
      // in a real app, you'd want to use a more robust solution
      await sleep(1)

      await router.navigate({ to: redirectTo })
    } catch (error) {
      console.error('Error logging in: ', error)
    } finally {
      setIsSubmitting(false)
    }
  }

  const isLoggingIn = isLoading || isSubmitting

  return (
    <div className="p-2 grid gap-2 place-items-center">
      <h3 className="text-xl">Login page</h3>
      {redirectTo && redirectTo !== '/dashboard' ? (
        <p className="text-red-500">You need to login to access this page.</p>
      ) : (
        <p>Login to see all the cool content in here.</p>
      )}
      <form className="mt-4 max-w-lg" onSubmit={onFormSubmit}>
        <fieldset disabled={isLoggingIn} className="w-full grid gap-2">
          <div className="grid gap-2 items-center min-w-[300px]">
            <label htmlFor="username-input" className="text-sm font-medium">
              Username
            </label>
            <input
              id="username-input"
              name="username"
              placeholder="Enter your name"
              type="text"
              className="border rounded-md p-2 w-full"
              required
            />
            <input
              id="password-input"
              name="password"
              placeholder="Enter your password"
              type="password"
              className="border rounded-md p-2 w-full"
              required
              
            />
          </div>
          <button
            type="submit"
            className="bg-blue-500 text-white py-2 px-4 rounded-md w-full disabled:bg-gray-300 disabled:text-gray-500"
          >
            {isLoggingIn ? 'Loading...' : 'Login'}
          </button>
        </fieldset>
      </form>
    </div>
  )
}