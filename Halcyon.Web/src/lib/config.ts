export const config = resolveRuntimeConfig({
    VERSION: import.meta.env.VERSION,
    VITE_RUNTIME_VALUE_1: import.meta.env.VITE_RUNTIME_VALUE_1,
    VITE_RUNTIME_VALUE_2: import.meta.env.VITE_RUNTIME_VALUE_2,
});

function resolveRuntimeConfig<T extends Record<string, string>>(source: T): T {
    const resolved: T = { ...source };

    for (const key of Object.keys(source)) {
        const runtimeValue = window.__ENV__?.[key];

        if (runtimeValue) {
            resolved[key as keyof T] = runtimeValue as T[keyof T];
        }
    }

    return resolved;
}
