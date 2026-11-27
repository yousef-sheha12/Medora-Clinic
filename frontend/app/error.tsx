"use client";

import { useEffect } from "react"; 
import Link from "next/link";
import StatusPage from "@/components/shared/StatusPage";
import { Button, buttonVariants } from "@/components/ui/button";
import { ROUTES } from "@/lib/constants";

export default function ErrorPage({
  error,
  reset, 
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    console.error(error); 
  }, [error]);

  return (
    <StatusPage
      title="Something went wrong"
      description="An unexpected error occurred. Please try again."
      actions={
        <>
          <Button onClick={reset}>Try again</Button>
          <Link href={ROUTES.home} className={buttonVariants({ variant: "outline" })}>
            Back to home
          </Link>
        </>
      }
    />
  );
}