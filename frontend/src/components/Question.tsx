import * as React from 'react';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import { useGetQuestionsQuery } from '../services/TriviaApi';
import { Refresh as RefreshIcon, Error as ErrorIcon, Quiz as QuizIcon } from '@mui/icons-material';

import { List, ListItem, Box, Stack, Chip, ListItemText, Divider, Alert, AlertTitle, CircularProgress, Container, Paper, ButtonGroup } from '@mui/material';
import { useState } from 'react';
// import { ScoreCounter } from './ScoreCounter';
import { useDispatch } from 'react-redux';
// import type { RootState } from '../Store';
import { increment } from './CounterSlice';
import { ScoreCounter } from './ScoreCounter';
const QuestionList: React.FC = () => {
  const { data, error, isLoading, refetch,  } = useGetQuestionsQuery();
  const [questionResult, setQuestionResult] = useState<String>("");
  const [questionIndex, setQuestionIndex] = useState<number>(0);
  const [score, setScore] = useState<number>(0);

  // const counter= useSelector((state:RootState) => state.counter.value);
  const dispatch = useDispatch();
  const handleAnswerClick = (selectedAnswer: string, correctAnswer: string) => {
    setQuestionIndex(questionIndex + 1);
    if(questionIndex== (data?.length))
      setQuestionIndex(0);
    if (selectedAnswer === correctAnswer) {
      console.log("correct");
      setQuestionResult("Correct");
      setScore(score + 1);
      dispatch(increment())
      // CounterSl
      return "correct";
    } else {
      setQuestionResult("False");
      console.log("false");
      return "false";
    }
  };

  if (isLoading) {
    return (
      <Box
        display="flex"
        flexDirection="column"
        alignItems="center"
        justifyContent="center"
        minHeight="100vh"
        bgcolor="grey.50"
      >
        <CircularProgress size={60} />
        <Typography variant="body1" color="text.secondary" sx={{ mt: 2 }}>
          Loading questions...
        </Typography>
      </Box>
    );
  }

  if (error) {
    const errorMessage = 'status' in error 
      ? `Error: ${error.status}` 
      : 'An error occurred';

    return (
      <Box
        display="flex"
        alignItems="center"
        justifyContent="center"
        minHeight="100vh"
        bgcolor="grey.50"
        p={2}
      >
        <Paper elevation={3} sx={{ p: 4, maxWidth: 500, width: '100%' }}>
          <Alert severity="error" icon={<ErrorIcon />}>
            <AlertTitle>Error</AlertTitle>
            {errorMessage}
          </Alert>
          <Button
            variant="contained"
            startIcon={<RefreshIcon />}
            onClick={() => refetch()}
            fullWidth
            sx={{ mt: 3 }}
          >
            Retry
          </Button>
        </Paper>
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
          {/* <Button
            variant="contained"
            startIcon={<RefreshIcon className={isFetching ? 'spinning' : ''} />}
            onClick={() => refetch()}
            disabled={isFetching}
          >
            Refresh
          </Button> */}
        </Box>

        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          Total Questions: {data?.length || 0}
        </Typography>
        <ScoreCounter />
        <Paper elevation={2}>
          {!data || data.length === 0 ? (
            <Box p={4} textAlign="center">
              <Typography variant="body1" color="text.secondary">
                No questions available
              </Typography>
            </Box>
          ) : (
            <List sx={{ p: 0 }}>
              {/* {data.map((question, index) => ( */}
                <React.Fragment key={data[questionIndex].id} >
                  <ListItem
                    sx={{
                      py: 3,
                      px: 3,
                      '&:hover': {
                        bgcolor: 'grey.50',
                      },
                      flexDirection: 'column',
                      alignItems: 'flex-start',
                    }}
                  >
                    
                    <Box width="100%">
                      <Stack direction="row" spacing={1} mb={1.5}>
                        <Chip 
                          label={data[questionIndex].questionTopic} 
                          color="primary" 
                          size="small"
                        />
                      </Stack>
                      
                      <ListItemText
                        primary={
                          <Typography variant="h6" component="div" fontWeight={600} mb={1}>
                            {data[questionIndex].questionDescription}
                          </Typography>
                        }
                        secondary={
                          <Box component="div">
                           
                            <Typography 
                              variant="body2" 
                              color="text.secondary"
                              sx={{ mt: 0.5 }}
                            >
                              <ButtonGroup variant="contained" aria-label="Basic button group">
                    {data[questionIndex].wrongAnswers.map((answer,i) => (
                      // <div key={index}>{question}</div>
  <Button key={i} onClick={ () => handleAnswerClick(answer,data[questionIndex].correctAnswer)}>{answer}</Button>
  // <Button>Two</Button>
  // <Button>Three</Button>
))}
</ButtonGroup>

  {questionResult== "Correct" &&    <Typography 
                              variant="body2" 
                              color="success.main" 
                              fontWeight={600}
                              sx={{ mt: 1 }}
                            >
                              
                              ✓ Correct!
                            </Typography> }
   {questionResult== "False" &&    <Typography 
                              variant="body2" 
                              color="failure.main" 
                              fontWeight={600}
                              sx={{ mt: 1 }}
                            >
                              
                              ❌ Wrong, the correct answer is : {data[questionIndex].correctAnswer}!
                            </Typography> }

                              {/* Wrong Answers: {question.wrongAnswers.join(', ')} */}
                            </Typography>
                            <Typography 
                              variant="caption" 
                              color="text.disabled"
                              sx={{ mt: 1, display: 'block' }}
                            >
                              {/* Created: {new Date(question.created).toLocaleDateString()} */}
                            </Typography>
                          </Box>
                        }
                      />
                    </Box>
                  </ListItem>
                  {questionIndex < data.length - 1 && <Divider component="li" />}
                </React.Fragment>
              {/* )
            )
              } */}
            </List>
          )}
        </Paper>
      </Container>

      <style>{`
        @keyframes spin {
          from { transform: rotate(0deg); }
          to { transform: rotate(360deg); }
        }
        .spinning {
          animation: spin 1s linear infinite;
        }
      `}</style>
    </Box>
  );
};

export default QuestionList;