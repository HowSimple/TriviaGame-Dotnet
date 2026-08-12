// import { useState } from "react"

import { Link } from "@mui/material"

import LoginButton from "./LoginButton"
import { useAuth0Context } from "../auth/Auth0"



export const NavigationBar = () => {
    // const [isAuthenticated] = useState(false)
    const auth = useAuth0Context()
    return (
        <nav className="bg-gray-800 p-4 text-white flex justify-between">
            <div className="text-lg font-bold">Trivia Game</div>
            <div>
                {auth.isAuthenticated ? (
                    <div >
  <Link href="/trivia/questions" color="inherit" className="ml-4">
                    Trivia
                </Link>
                                      <p>Welcome back, {auth.user?.name ?? auth.user?.email}!</p>

                                    <button onClick={() => auth.logout()}>Logout </button>

                        </div>
                    
                ) : (
                    // <button onClick={() => auth.login()}>Login</button>
                    <LoginButton />
                )}
            
              
            </div>
        </nav>
    )
}  