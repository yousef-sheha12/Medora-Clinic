import {
    Bell,
    CalendarDays,
    Clock,
    CreditCard,
    LayoutDashboard,
    MapPin,
    Search,
    Stethoscope,
    User,
    Users,
    type LucideIcon,
} from "lucide-react";

import { ROLES, type Role } from "@/lib/constants";

export interface NavItem {
    label: string;
    href: string;
    icon: LucideIcon;
}

export const NAV_ITEMS: Record<Role, NavItem[]> = {
    [ROLES.PATIENT]: [
        { label: "Find doctors", href: "/doctors", icon: Search },
        { label: "My appointments", href: "/patient/appointments", icon: CalendarDays },
        { label: "Notifications", href: "/patient/notifications", icon: Bell },
        { label: "Profile", href: "/patient/profile", icon: User },
    ],
    [ROLES.DOCTOR]: [
        { label: "Dashboard", href: "/doctor/dashboard", icon: LayoutDashboard },
        { label: "Availability", href: "/doctor/availability", icon: Clock },
        { label: "Appointments", href: "/doctor/appointments", icon: CalendarDays },
        { label: "Notifications", href: "/doctor/notifications", icon: Bell },
        { label: "Profile", href: "/doctor/profile", icon: User },
    ],
    [ROLES.ADMIN]: [
        { label: "Dashboard", href: "/admin/dashboard", icon: LayoutDashboard },
        { label: "Users", href: "/admin/users", icon: Users },
        { label: "Specialties", href: "/admin/specialties", icon: Stethoscope },
        { label: "Locations", href: "/admin/locations", icon: MapPin },
        { label: "Appointments", href: "/admin/appointments", icon: CalendarDays },
        { label: "Payments", href: "/admin/payments", icon: CreditCard },
    ],
};
