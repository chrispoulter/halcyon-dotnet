/// <reference types="vite/client" />

declare module 'ky' {
    interface KyOptions {
        context?: {
            accessToken?: string;
        };
    }
}
