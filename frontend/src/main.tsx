import React from 'react'
import ReactDOM from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'

import { routeTree } from './routeTree.gen'
// import { AuthProvider, useAuth } from './auth'
import './styles.css'
import { store } from './Store'
import { Provider } from 'react-redux'
import { useAuth0Context } from './auth/Auth0'
import { Auth0Wrapper } from './auth/Auth0'
// Set up a Router instance
const router = createRouter({
  routeTree,
  defaultPreload: 'intent',
  scrollRestoration: true,
  context: {
    auth: undefined!, // This will be set after we wrap the app in an AuthProvider
  },
})

// Register things for typesafety
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

function InnerApp() {
  const auth = useAuth0Context()
  return <RouterProvider router={router} context={{ auth }} />
}

function App() {
  return (
    <Auth0Wrapper
    
    
    >
      <InnerApp />
    </Auth0Wrapper>
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
