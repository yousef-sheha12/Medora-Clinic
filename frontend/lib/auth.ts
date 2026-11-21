const ACCESS_KEY = "clinic_access_token";
const REFRESH_KEY = "clinic_refresh_token";

const isBrowser = () => typeof window !== "undefined";

export function getAccessToken(): string | null {
    if (!isBrowser()) return null;
    try {
        return localStorage.getItem(ACCESS_KEY);
    } catch {
        return null;
    }
}

export function getRefreshToken(): string | null {
    if(!isBrowser()) return null;
    try {
        return localStorage.getItem(REFRESH_KEY);
    } catch {
        return null;
    }
}

export function setTokens(access: string, refresh?: string): void {
    if(!isBrowser()) return;
    try {
        localStorage.setItem(ACCESS_KEY, access);
        if (refresh) localStorage.setItem(REFRESH_KEY, refresh);
    } catch {

    }
}

export function clearTokens(): void {
    if (!isBrowser()) return;
    try {
        localStorage.removeItem(ACCESS_KEY);
        localStorage.removeItem(REFRESH_KEY);
    } catch {

    }
}