import React from 'react'
import ReactDOM from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'

import { routeTree } from './routeTree.gen'
// import { AuthProvider, useAuth } from './auth'
import './styles.css'
import { store } from './Store'
import { Provider } from 'react-redux'


import { AuthProvider, useAuth, type AuthContext } from './auth'
import { Auth0Wrapper } from './auth/Auth0'
import { NavigationBar } from './components/NavigationBar'
// Set up a Router instance
const router = createRouter({
  routeTree,
  defaultPreload: 'intent',
  scrollRestoration: true,
  context: {
    auth: undefined! as AuthContext, // This will be set after we wrap the app in an AuthProvider
  },
})

// Register things for typesafety
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

function InnerApp() {
  // const auth0 = useAuth0()
  // const auth = useAuth0Context()
  const auth = useAuth()  
  // const auth = {  
  //   ...auth0,
  //   login:  async (username:string) => {
  //     await auth0.loginWithRedirect({
  //       authorizationParams: {
  //         login_hint: username, // Pass the username as a hint to Auth0

  //         redirect_uri: window.location.origin,
  //       }
  //     })  
  //   },
    // logout: async () =>
    //   await auth0.logout({ logoutParams: { returnTo: window.location.origin } }),
  // }
  return <RouterProvider router={router} context={{ auth }} />
}

function App() {
  return (
    < Auth0Wrapper>
    <AuthProvider>
       <NavigationBar />
      <InnerApp>
      </InnerApp>
    </AuthProvider>
     </Auth0Wrapper  >
  )
}
    
    
  


const rootElement = document.getElementById('app')!

if (!rootElement.innerHTML) {
  const root = ReactDOM.createRoot(rootElement)
  root.render(
    
    <React.StrictMode>
      <Provider store={store}>
      <App />
      </Provider>
    </React.StrictMode>,
  )
}
