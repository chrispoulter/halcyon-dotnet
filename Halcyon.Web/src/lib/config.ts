export const config = {
    VERSION: String(import.meta.env.VERSION),
    VITE_API_URL:
        String(import.meta.env.VITE_API_URL) || `${window.location.origin}/api`,
};
