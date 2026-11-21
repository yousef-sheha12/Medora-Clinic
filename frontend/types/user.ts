import type { Role } from "@/lib/constants";

export interface User {
    id: number;
    email: string;
    role: Role;
    is_blocked: boolean;
    date_joined: string;
}