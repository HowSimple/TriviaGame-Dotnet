import { createFileRoute } from '@tanstack/react-router'
import Question from '../components/Question'

export const Route = createFileRoute('/_auth/trivia/questions')({
  component: RouteComponent,
})

function RouteComponent() {
 return <div>

    <Question />  
  </div>
}
