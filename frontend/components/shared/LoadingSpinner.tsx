import { Loader2 } from "lucide-react";
export default function LoadingSpinner({ label = "Loading..."}: { label?: string }) {
    return (
    <div role="status" className="flex items-center justify-center gap-2 p-8 text-muted-foreground">
      <Loader2 className="size-5 animate-spin" aria-hidden="true" /> 
      <span className="text-sm">{label}</span>
    </div>
  );
}


