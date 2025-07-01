export const config = {
    VERSION: import.meta.env.VERSION,
    VITE_API_URL:
        import.meta.env.VITE_API_URL || `${window.location.origin}/api`,
};
