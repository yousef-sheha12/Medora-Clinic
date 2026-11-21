import { ROUTES } from "@/lib/constants";
import { clearTokens, getAccessToken, getRefreshToken, setTokens } from "@/lib/auth";
import { ApiError } from "@/types/api";

const BASE_URL = process.env.NEXT_PUBLIC_API_URL ?? "";

export class HttpError extends Error implements ApiError {
	status: number;
    fieldErrors?: Record<string, string[]>;

	constructor(status: number, message: string, fieldErrors?: Record<string, string[]>) {
		super(message);
		this.name = "HttpError";
		this.status = status;
		this.fieldErrors = fieldErrors;
	}
}

function defaultMessage(status: number): string {
  if (status === 401) return "Your session has expired. Please log in again.";
  if (status === 403) return "You don't have permission to do this.";
  if (status === 404) return "The requested item was not found.";
  if (status === 409) return "This action conflicts with the current data (e.g. the slot was just booked).";
  if (status >= 500) return "Server error. Please try again later.";
  return "Something went wrong. Please try again.";
}

async function parseError(res: Response): Promise<HttpError> {
    let body: unknown = null;
    try {
        body = await res.json();
    } catch {
        // Ignore JSON parse errors
    }
    const data = (body ?? {}) as Record<string, unknown>;

    if (typeof data.detail === "string") {
        return new HttpError(res.status, data.detail);
    }

    const fieldErrors: Record<string, string[]> = {};
    for (const [key, value] of Object.entries(data)){
        if (Array.isArray(value)) fieldErrors[key] = value.map(String);
        else if (typeof value === "string") fieldErrors[key] = [value];
    }
    const hasFields = Object.keys(fieldErrors).length > 0;

    return new HttpError(
        res.status,
        hasFields ? "Please check the highlighted fields." : defaultMessage(res.status),
        hasFields ? fieldErrors : undefined
    );
} 

let refreshPromise: Promise<boolean> | null = null;

async function refreshAccessToken(): Promise<boolean> {
    if (refreshPromise) return refreshPromise;

    refreshPromise = (async () => {
        const refresh = getRefreshToken();
        if (!refresh) return false;

        try {
            const res = await fetch(`$(BASE_URL)/auth/refresh`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ refresh }),
            });
            if (!res.ok) return false;

            const data = await res.json();
            setTokens(data.access, data.refresh);
            return true;
        }catch {
            return false;
        }
    })();

    try {
        return await refreshPromise;
    } finally {
        refreshPromise = null;
    }
}

type RequestOptions = {
    method?: "GET" | "POST" | "PUT" | "PATCH" | "DELETE";
    body?: unknown;
    params?: Record<string, string | number | boolean | null | undefined>;
    auth?: boolean;
}

function buildUrl(path: string, params?: RequestOptions["params"]): string {
  const url = `${BASE_URL}${path}`;
  if (!params) return url;

  const search = new URLSearchParams();
  for (const [key, value] of Object.entries(params)) {
    if (value !== undefined && value !== null && value !== "") {
      search.append(key, String(value));
    }
  }
  const qs = search.toString();
  return qs ? `${url}?${qs}` : url;
}

async function request<T>(
  path: string,
  options: RequestOptions = {},
  hasRetried = false 
): Promise<T> {
  const { method = "GET", body, params, auth = true } = options;

  const headers: Record<string, string> = { "Content-Type": "application/json" };
  const token = auth ? getAccessToken() : null;
  if (token) headers.Authorization = `Bearer ${token}`; 

  let res: Response;
  try {
    res = await fetch(buildUrl(path, params), {
      method,
      headers,
      body: body !== undefined ? JSON.stringify(body) : undefined, 
    });
  } catch {
    throw new HttpError(0, "Cannot reach the server. Check your connection and try again.");
  }

  if (res.status === 401 && auth && !hasRetried) {
    const refreshed = await refreshAccessToken();
    if (refreshed) return request<T>(path, options, true);
    clearTokens();
    if (typeof window !== "undefined" && window.location.pathname !== ROUTES.login) {
      window.location.href = ROUTES.login;
    }
    throw await parseError(res);
  }

  if (!res.ok) throw await parseError(res);
  if (res.status === 204) return undefined as T;  
  return (await res.json()) as T; 
}

type AuthOption = Pick<RequestOptions, "auth">;

export const api = {
  get: <T>(path: string, params?: RequestOptions["params"], opts?: AuthOption) =>
    request<T>(path, { method: "GET", params, ...opts }),

  post: <T>(path: string, body?: unknown, opts?: AuthOption) =>
    request<T>(path, { method: "POST", body, ...opts }),

  put: <T>(path: string, body?: unknown, opts?: AuthOption) =>
    request<T>(path, { method: "PUT", body, ...opts }),

  patch: <T>(path: string, body?: unknown, opts?: AuthOption) =>
    request<T>(path, { method: "PATCH", body, ...opts }),

  delete: <T>(path: string, opts?: AuthOption) =>
    request<T>(path, { method: "DELETE", ...opts }),
};