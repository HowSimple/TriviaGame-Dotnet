import * as React from 'react';
import {
  Box,
  Stack,
  Chip,
  ListItemText,
  Typography,
  ButtonGroup,
  Button,
} from '@mui/material';
import type { TriviaQuestion } from '../models/TriviaQuestion';


interface QuestionProps extends TriviaQuestion {
  onAnswerClick: (selectedAnswer: string, correctAnswer: string) => void;
  questionResult: string;
}

export const Question: React.FC<QuestionProps> = (props) => {
  return (
    <Box width="100%">
      <Stack direction="row" spacing={1} mb={1.5}>
        <Chip label={props.questionTopic} color="primary" size="small" />
      </Stack>

      <ListItemText
        primary={
          <Typography variant="h6" component="div" fontWeight={600} mb={1}>
            {props.questionDescription}
          </Typography>
        }
        secondary={
          <Box component="div">
            <Typography variant="body2" color="text.secondary" sx={{ mt: 0.5 }}>
              <ButtonGroup variant="contained" aria-label="Answer options">
                {props.wrongAnswers.map((answer, i) => (
                  <Button
                    key={i}
                    onClick={() => props.onAnswerClick(answer, props.correctAnswer)}
                  >
                    {answer}
                  </Button>
                ))}
              </ButtonGroup>

              {props.questionResult === 'Correct' && (
                <Typography
                  variant="body2"
                  color="success.main"
                  fontWeight={600}
                  sx={{ mt: 1 }}
                >
                  ✓ Correct!
                </Typography>
              )}

              {props.questionResult === 'False' && (
                <Typography
                  variant="body2"
                  color="error.main"
                  fontWeight={600}
                  sx={{ mt: 1 }}
                >
                  ❌ Wrong, the correct answer is: {props.correctAnswer}!
                </Typography>
              )}
            </Typography>
          </Box>
        }
      />
    </Box>
  );
};

export default Question;