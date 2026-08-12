import { createFileRoute } from '@tanstack/react-router'
import { Link } from '@tanstack/react-router'
// import { useAuth0Context } from '../auth/Auth0'
import LoginButton from '../components/LoginButton'
import { useAuth } from '../auth'
import { NavigationBar } from '../components/NavigationBar'

export const Route = createFileRoute('/')({
  component: HomeComponent,
})

function HomeComponent() {
  // const auth = useAuth0Context()
  const auth = useAuth()

  return (
    <div className="p-2 grid gap-2">
      <NavigationBar />
      {/* <h1 className="text-xl">Welcome!</h1> */}
      <p>
        You are currently on the index2 route.
        
      </p>
      
      {auth.isAuthenticated ? (
        <>
          <p>Welcome back, {auth.user}!</p>
          {/* <p>You can try going through these options:</p> */}
          <ol className="list-disc list-inside px-2">
          
            
            <li>
              {/* <LoginButton /> to log out. */}
              <Link to="/trivia/questions" className="text-blue-500 hover:opacity-75">
                Go to the trivia questions page.
              </Link>
            </li>
          </ol>
        </>
      ) : (
        <>
          <p>Please  to access protected routes.</p>
          <ol className="list-disc list-inside px-2">

            <li>
              <Link to="/login" className="text-blue-500 hover:opacity-75">
                Go to the login page.
              </Link>
              {/* <LoginButton />   to log in. */}
    

            </li>
            
          </ol>
        </>
      )}
    </div>
  )
}
