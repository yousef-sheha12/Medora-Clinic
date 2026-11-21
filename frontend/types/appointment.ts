import type { AppointmentStatus } from "@/lib/constants"; 
import type { Availability } from "./availability";       
import type { DoctorProfile, PatientProfile } from "./doctor";

export interface Appointment {
  id: number;              
  patient_id: number; 
  doctor_id: number;
  availability_id: number | null; 
  status: AppointmentStatus;
  notes: string; 
  created_at: string;
  updated_at: string; 
  doctor?: DoctorProfile;
  patient?: PatientProfile;
  availability?: Availability;
}

export interface BookAppointmentPayload {
  availability_id: number; 
}

export interface UpdateAppointmentPayload {
  status: AppointmentStatus;
  notes?: string;
  reason?: string;
}