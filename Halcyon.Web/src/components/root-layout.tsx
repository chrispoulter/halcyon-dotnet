import { Outlet, useLocation } from 'react-router';
import { ErrorBoundary } from 'react-error-boundary';
import { ErrorPage } from '@/error-page';
import { Header } from './header';
import { Footer } from './footer';

export function RootLayout() {
    const { pathname } = useLocation();

    return (
        <>
            <Header />
            <ErrorBoundary FallbackComponent={ErrorPage} resetKeys={[pathname]}>
                <Outlet />
            </ErrorBoundary>
            <Footer />
        </>
    );
}
