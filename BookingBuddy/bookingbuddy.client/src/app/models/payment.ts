export interface Payment {
  paymentId: string;
  method: string;
  amount: number;
  entity?: string;
  reference?: string;
  expiryDate?: Date;
  createdAt: Date;
  status: string;
}
