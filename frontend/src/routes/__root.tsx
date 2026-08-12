import { Outlet, createRootRouteWithContext } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools'
import type { AuthContext } from '../auth'
// import type { Auth0ContextType } from '../auth/Auth0'



interface MyRouterContext {
  auth: AuthContext // This will be provided by the AuthProvider in main.tsx  
}

export const Route = createRootRouteWithContext<MyRouterContext>()({
  component: () => (
    <>
      <Outlet />
      <TanStackRouterDevtools position="bottom-right" initialIsOpen={false} />
    </>
  ),
})
