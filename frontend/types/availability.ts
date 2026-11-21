export interface Availability {
  id: number;                  
  doctor_id: number;           
  location_id?: number | null; 
  date: string;                
  start_time: string;          
  end_time: string;            
  is_booked: boolean;          
}