"use client";

import Link from "next/link";
import StatusPage from "@/components/shared/StatusPage";
import { buttonVariants } from "@/components/ui/button";
import { useAuth } from "@/providers/AuthProvider";
import { ROLE_HOME, ROUTES } from "@/lib/constants";

export default function ForbiddenPage() {
  const { role } = useAuth(); 
  const homeHref = role ? ROLE_HOME[role] : ROUTES.home;

  return (
    <StatusPage
      code="403"
      title="Access denied"
      description="You don't have permission to view this page."
      actions={
        <Link href={homeHref} className={buttonVariants()}>
          {role ? "Go to my dashboard" : "Back to home"}
        </Link>
      }
    />
  );
}