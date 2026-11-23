export const ROLES ={
    ADMIN: "admin",
    DOCTOR: "doctor",
    PATIENT: "patient",
} as const;

export type Role = (typeof ROLES)[keyof typeof ROLES];

export const APPOINTMENT_STATUS ={
    PENDING: "pending",
    CONFIRMED: "confirmed",
    COMPLETED: "completed",
    CANCELLED: "cancelled",
    
} as const;

export type AppointmentStatus = (typeof APPOINTMENT_STATUS)[keyof typeof APPOINTMENT_STATUS];

export const ROUTES = {
    home: "/",
    login: "/login",
    register: "/register",
    verifyEmail: "/verify-email",
    forgetPassword: "/forget-password",
    resetPassword: "/reset-password",
    doctors: "/doctors",
    forbidden: "/forbidden",
    patientHome: "/patient/appointments",
    doctorHome: "/doctor/appointments",
    adminHome: "/admin/dashboard",
} as const;

export const ROLE_HOME: Record<Role, string> = {
  [ROLES.ADMIN]: ROUTES.adminHome,
  [ROLES.DOCTOR]: ROUTES.doctorHome,
  [ROLES.PATIENT]: ROUTES.patientHome,
};