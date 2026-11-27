import Image from "next/image";
import type { ReactNode } from "react";

interface StatusPageProps {
    code?: string;
    title: string;
    description: string;
    actions?: ReactNode;
}

export default function StatusPage({ code, title, description, actions }:StatusPageProps){
    return (
    <main className="flex min-h-screen flex-col items-center justify-center bg-gradient-to-b from-secondary to-background px-4 text-center">
      <Image src="/logo.png" alt="" width={64} height={64} />
      {code && <p className="mt-6 text-6xl font-bold text-primary">{code}</p>}
      <h1 className="mt-2 text-2xl font-semibold">{title}</h1>
      <p className="mt-2 max-w-md text-muted-foreground">{description}</p>
      {actions && <div className="mt-6 flex flex-wrap justify-center gap-3">{actions}</div>}
    </main>
  );
}