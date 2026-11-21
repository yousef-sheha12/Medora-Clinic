export interface Specialty {
    id: number;
    name: string;
    description: string;
}

export  interface Location {
    id: number;
    name: string;
    address: string;
    city: string;
    phone: string;
}

export interface DoctorProfile {
    id: number;
    user_id: number;
    full_name: string;
    specialty: Specialty;
    price: number;
    bio: string;
    contact_number: string;
    is_approved: boolean;
    rating?: number;
    location?: Location[];
}

export interface PatientProfile {
    id: number;
    user_id: number;
    full_name: string;
    contact_number: string;
    date_of_birth: string;
    address: string;
    is_approved: boolean;
}
