declare global {
    interface Window {
        __ENV__?: Record<string, string>;
    }
}

export const config = resolveRuntimeConfig({
    API_URL: `${window.location.origin}/api`,
    VERSION: import.meta.env.VERSION,
    VITE_RUNTIME_VALUE_1: import.meta.env.VITE_RUNTIME_VALUE_1,
    VITE_RUNTIME_VALUE_2: import.meta.env.VITE_RUNTIME_VALUE_2,
});

function resolveRuntimeConfig<T extends Record<string, string>>(source: T): T {
    const env = window.__ENV__ || {};
    const resolved: T = { ...source };

    for (const [key, value] of Object.entries(env)) {
        if (!env[key].startsWith('${')) {
            resolved[key as keyof T] = value as T[keyof T];
        }
    }

    return resolved;
}
