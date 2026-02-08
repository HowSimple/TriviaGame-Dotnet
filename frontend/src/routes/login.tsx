import { createFileRoute, redirect } from '@tanstack/react-router'
import { useAuth } from '../auth'
import { useState } from 'react'

export const Route = createFileRoute('/login')({
  beforeLoad: ({ context, search }) => {
    if (context.auth.isAuthenticated) {
      throw redirect({ to: search.redirect || '/dashboard' })
    }
  },
  component: LoginPage,
})

function LoginPage() {
  const auth = useAuth()
  const navigate = Route.useNavigate()
  const search = Route.useSearch()
  const [username, setUsername] = useState('')

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    if (username.trim()) {
      await auth.login(username)
      navigate({ to: search.redirect || '/dashboard' })
    }
  }

  return (
    <div className="p-8 max-w-md mx-auto">
      <h1 className="text-2xl font-bold mb-4">Login</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label htmlFor="username" className="block mb-2">
            Username:
          </label>
          <input
            id="username"
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full px-4 py-2 border rounded"
            placeholder="Enter your username"
            autoFocus
          />
        </div>
        <button
          type="submit"
          className="w-full px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
        >
          Login
        </button>
      </form>
    </div>
  )
}
