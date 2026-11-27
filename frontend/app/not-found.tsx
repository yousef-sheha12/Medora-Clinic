import Link from "next/link";
import StatusPage from "@/components/shared/StatusPage";
import { buttonVariants } from "@/components/ui/button";
import { ROUTES } from "@/lib/constants"; 

export default function NotFound() {
  return (
    <StatusPage
      code="404"
      title="Page not found"
      description="The page you are looking for doesn't exist or has been moved."
      actions={
        <Link href={ROUTES.home} className={buttonVariants()}>
          Back to home
        </Link>
      }
    />
  );
}