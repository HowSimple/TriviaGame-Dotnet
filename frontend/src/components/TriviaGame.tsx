import * as React from 'react';
import { useState } from 'react';
import { useDispatch } from 'react-redux';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import {
  Box, Container, Paper, List, ListItem,
  CircularProgress, Alert, AlertTitle,
} from '@mui/material';
import {
  Refresh as RefreshIcon,
  Error as ErrorIcon,
  Quiz as QuizIcon,
} from '@mui/icons-material';

import { useGetQuestionsQuery, useGetMyScoreQuery, useSaveHighScoreMutation } from '../services/TriviaApi';
import { increment } from './CounterSlice';
import { ScoreCounter } from './ScoreCounter';
import { useAuth0Context } from '../auth/Auth0';
import Question from './Question';

const TriviaGame: React.FC = () => {
  const { data, error, isLoading, refetch } = useGetQuestionsQuery();
  const { data: myScore } = useGetMyScoreQuery();
  const [saveHighScore, { isLoading: isSaving }] = useSaveHighScoreMutation();
  const { user } = useAuth0Context();
  const dispatch = useDispatch();

  const [score, setScore] = useState(0);
  const [questionIndex, setQuestionIndex] = useState(0);
  const [questionResult, setQuestionResult] = useState('');
  const [gameOver, setGameOver] = useState(false);
  const [scoreSaved, setScoreSaved] = useState(false);

  const handleAnswerClick = async (selectedAnswer: string, correctAnswer: string) => {
    const isCorrect = selectedAnswer === correctAnswer;
    const newScore = isCorrect ? score + 1 : score;

    if (isCorrect) {
      setQuestionResult('Correct');
      setScore(newScore);
      dispatch(increment());
    } else {
      setQuestionResult('False');
    }

    const nextIndex = questionIndex + 1;

    if (data && nextIndex >= data.length) {
      setGameOver(true);
      try {
        await saveHighScore({
          score: newScore,
          userName: user?.name ?? user?.email,
        });
        setScoreSaved(true);
      } catch {
        // save failed silently
      }
    } else {
      setQuestionIndex(nextIndex);
    }
  };

  if (isLoading) {
    return (
      <Box display="flex" flexDirection="column" alignItems="center" justifyContent="center" minHeight="100vh" bgcolor="grey.50">
        <CircularProgress size={60} />
        <Typography variant="body1" color="text.secondary" sx={{ mt: 2 }}>
          Loading questions...
        </Typography>
      </Box>
    );
  }

  if (error) {
    const errorMessage = 'status' in error ? `Error: ${error.status}` : 'An error occurred';
    return (
      <Box display="flex" alignItems="center" justifyContent="center" minHeight="100vh" bgcolor="grey.50" p={2}>
        <Paper elevation={3} sx={{ p: 4, maxWidth: 500, width: '100%' }}>
          <Alert severity="error" icon={<ErrorIcon />}>
            <AlertTitle>Error</AlertTitle>
            {errorMessage}
          </Alert>
          <Button variant="contained" startIcon={<RefreshIcon />} onClick={() => refetch()} fullWidth sx={{ mt: 3 }}>
            Retry
          </Button>
        </Paper>
      </Box>
    );
  }

  if (gameOver) {
    return (
      <Box display="flex" flexDirection="column" alignItems="center" justifyContent="center" minHeight="60vh" gap={2}>
        <Typography variant="h4">Game Over!</Typography>
        <Typography variant="h5">Final score: {score} / {data?.length}</Typography>
        {isSaving && <Typography variant="body2" color="text.secondary">Saving score...</Typography>}
        {scoreSaved && <Typography variant="body2" color="success.main">✓ High score saved!</Typography>}
        <Button
          variant="contained"
          onClick={() => {
            setQuestionIndex(0);
            setScore(0);
            setGameOver(false);
            setScoreSaved(false);
            setQuestionResult('');
          }}
        >
          Play again
        </Button>
      </Box>
    );
  }

  return (
    <Box minHeight="100vh" bgcolor="grey.50" py={4}>
      <Container maxWidth="lg">
        <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
          <Box display="flex" alignItems="center" gap={2}>
            <QuizIcon sx={{ fontSize: 40, color: 'primary.main' }} />
            <Typography variant="h4" component="h1" fontWeight="bold">
              Movie Quiz Questions
            </Typography>
          </Box>
        </Box>

        <Typography variant="body1" color="text.secondary" sx={{ mb: 1 }}>
          Question {questionIndex + 1} of {data?.length || 0}
        </Typography>
        <ScoreCounter />

        <Box sx={{ display: 'flex', gap: 4, mb: 3 }}>
          <Typography variant="body2" color="text.secondary">
            Current game: {score} point{score !== 1 ? 's' : ''}
          </Typography>
          {myScore !== undefined && (
            <Typography variant="body2" color="primary">
              Personal best: {myScore.score}
            </Typography>
          )}
        </Box>

        <Paper elevation={2}>
          {!data || data.length === 0 ? (
            <Box p={4} textAlign="center">
              <Typography variant="body1" color="text.secondary">No questions available</Typography>
            </Box>
          ) : (
            <List sx={{ p: 0 }}>
              <React.Fragment key={data[questionIndex].id}>
                <ListItem sx={{ py: 3, px: 3, flexDirection: 'column', alignItems: 'flex-start' }}>
                  <Question
                    {...data[questionIndex]}
                    onAnswerClick={handleAnswerClick}
                    questionResult={questionResult}
                  />
                </ListItem>
              </React.Fragment>
            </List>
          )}
        </Paper>
      </Container>
    </Box>
  );
};

export default TriviaGame;