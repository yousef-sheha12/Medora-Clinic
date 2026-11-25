import { AlertCircle } from "lucide-react";
import { Button } from "@/components/ui/button";

interface ErrorMessageProps {
    error?: unknown;
    message?: string;
    onRetry?: ()=> void;
}

function getMessage(error: unknown, message?: string): string {
    if (message) return message;
    if (error instanceof Error) return error.message;
    return "Something went wrong. Please try again.";
}

export default function ErrorMessage({ error, message, onRetry }: ErrorMessageProps) {
    return (
    <div
      role="alert"
      className="flex flex-col items-center gap-3 rounded-lg border border-destructive/30 bg-destructive/5 p-6 text-center"
    >
      <AlertCircle className="size-8 text-destructive" aria-hidden="true" />
      <p className="text-sm text-destructive">{getMessage(error, message)}</p>
      {onRetry && (
        <Button variant="outline" size="sm" onClick={onRetry}>
          Try again
        </Button>
      )}
    </div>
  );
}