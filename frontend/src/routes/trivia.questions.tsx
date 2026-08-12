import { createFileRoute } from '@tanstack/react-router'
import TriviaGame from '../components/TriviaGame'

export const Route = createFileRoute('/trivia/questions')({
  component: TriviaComponent,
})

function TriviaComponent() {
  //  const auth = useAuth()
 return <div>

    <TriviaGame />  
  </div>
}
