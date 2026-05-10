import { z } from 'zod';

declare global {
    interface Window {
        __ENV__?: Record<string, string>;
    }
}

const configSchema = z.object({
    VITE_APP_VERSION: z.string(),
    VITE_API_URL: z.url(),
    VITE_RUNTIME_VALUE_1: z.string().optional(),
    VITE_RUNTIME_VALUE_2: z.string().optional(),
});

export const config = configSchema.parse({
    ...import.meta.env,
    ...window.__ENV__,
});
