"use client";

import { useState, type ReactNode } from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { HttpError } from "@/lib/api-client";

export default function QueryProvider({ children }:{ children: ReactNode }) {

    const [client] = useState(() => new QueryClient({
        defaultOptions: {
            queries: {
                staleTime: 60 * 1000,
                refetchOnWindowFocus: false,
                retry: (failureCount, error) => {
                    if (error instanceof HttpError && error.status >= 400 && error.status < 500){
                        return false;
                    }
                    return failureCount < 2;
                },
            },
        },
    })
    );
    return <QueryClientProvider client={client}>{children}</QueryClientProvider>
}

