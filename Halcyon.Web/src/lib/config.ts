declare global {
    interface Window {
        __ENV__?: Record<string, string>;
    }
}

export const config = {
    API_URL: `${window.location.origin}/api`,
    VERSION: import.meta.env.VERSION,
    ...resolveRuntimeConfig({
        VITE_RUNTIME_VALUE_1: import.meta.env.VITE_RUNTIME_VALUE_1,
        VITE_RUNTIME_VALUE_2: import.meta.env.VITE_RUNTIME_VALUE_2,
    }),
};

function resolveRuntimeConfig<T extends Record<string, string>>(source: T): T {
    const resolved: T = { ...source };

    for (const key of Object.keys(source)) {
        const runtimeValue = window.__ENV__?.[key];

        if (runtimeValue && !runtimeValue.startsWith('${')) {
            resolved[key as keyof T] = runtimeValue as T[keyof T];
        }
    }

    return resolved;
}
