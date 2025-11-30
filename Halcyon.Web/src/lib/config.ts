declare global {
    interface Window {
        __ENV__: Record<string, string>;
    }
}

export const config = resolveRuntimeConfig({
    API_URL: `${window.location.origin}/api`,
    VERSION: import.meta.env.VERSION,
    VITE_RUNTIME_VALUE_1: import.meta.env.VITE_RUNTIME_VALUE_1,
    VITE_RUNTIME_VALUE_2: import.meta.env.VITE_RUNTIME_VALUE_2,
});

function resolveRuntimeConfig(source: Record<string, string>) {
    if (!window.__ENV__) {
        return source;
    }

    const resolved = { ...source };

    for (const [key, value] of Object.entries(window.__ENV__)) {
        if (!value.startsWith('${')) {
            resolved[key] = value;
        }
    }

    return resolved;
}
