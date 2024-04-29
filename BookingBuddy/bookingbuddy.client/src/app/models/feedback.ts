export interface Feedback {
  feedback: string;
  type: FeedbackType;
}

export enum FeedbackType {
  SUCCESS = 'success',
  ERROR = 'error',
  INFO = 'info',
  WARNING = 'warning'
}
