export interface Payment {
  id: number;
  patient_id: number;
  appointment_id: number;
  paymentMethod: string;
  paymentDate: string;
  amount: number;
}

export interface CreatePaymentPayload {
  paymentMethod: string;
  amount: number;
}