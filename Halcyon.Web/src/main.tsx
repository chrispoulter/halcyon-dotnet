import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router';
import { ErrorBoundary } from 'react-error-boundary';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { Toaster } from '@/components/ui/sonner';
import { AuthProvider } from '@/components/auth-provider';
import { ThemeProvider } from '@/components/theme-provider';
import App from '@/app';

import '@/index.css';

const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            staleTime: 1000 * 10,
            retry: false,
            refetchOnWindowFocus: false,
        },
    },
});

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <QueryClientProvider client={queryClient}>
            <BrowserRouter>
                <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
                    <ErrorBoundary
                        fallback={
                            <div className="flex min-h-screen items-center justify-center p-8 text-center">
                                Something went wrong. Please refresh the page.
                            </div>
                        }
                    >
                        <AuthProvider>
                            <App />
                        </AuthProvider>
                    </ErrorBoundary>
                </ThemeProvider>
            </BrowserRouter>
            <Toaster invert />
            <ReactQueryDevtools initialIsOpen={false} />
        </QueryClientProvider>
    </StrictMode>
);
