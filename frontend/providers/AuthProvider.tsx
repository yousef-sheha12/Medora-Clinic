"use client";

import {
  createContext,
  useCallback,
  useContext,
  useMemo,
  useSyncExternalStore,
  type ReactNode,
} from "react";

import { useRouter } from "next/navigation";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { api } from "@/lib/api-client";
import { clearTokens, getAccessToken, setTokens } from "@/lib/auth";
import { ROUTES, type Role } from "@/lib/constants";
import type { User } from "@/types/user";

const ME_KEY = ["me"] as const;

const fetchMe = () => api.get<User>("/auth/me");

interface AuthContextValue {
  user: User | null;
  role: Role | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  login: (access: string, refresh?: string) => Promise<User>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

const subscribe = () => () => {};

function useIsClient() {
  return useSyncExternalStore(
    subscribe,
    () => true,
    () => false,
  );
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const queryClient = useQueryClient();
  const router = useRouter();
  const isClient = useIsClient();
  const hasToken = isClient && !!getAccessToken();

  const { data, isLoading: isFetchingUser } = useQuery<User | null>({
    queryKey: ME_KEY,
    queryFn: fetchMe,
    enabled: hasToken,
    retry: false,
    staleTime: 5 * 60 * 1000,
  });

  const user = data ?? null;
  const role = user?.role ?? null;

  const login = useCallback(
    async (access: string, refresh?: string) => {
      setTokens(access, refresh);
      try {
        return await queryClient.fetchQuery({
          queryKey: ME_KEY,
          queryFn: fetchMe,
          staleTime: 0,
        });
      } catch (error) {
        clearTokens();
        throw error;
      }
    },
    [queryClient],
  );

  const logout = useCallback(() => {
    clearTokens();
    queryClient.setQueryData<User | null>(ME_KEY, null);
    queryClient.removeQueries({
      predicate: (query) => query.queryKey[0] !== ME_KEY[0],
    });
    router.replace(ROUTES.login);
  }, [queryClient, router]);
  
  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      role,
      isAuthenticated: !!user,
      isLoading: !isClient || (hasToken && isFetchingUser),
      login,
      logout,
    }),
    [user, role, isClient, hasToken, isFetchingUser, login, logout],
  );
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): AuthContextValue {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error("useAuth must be used inside <AuthProvider>");
  }
  return ctx;
}
