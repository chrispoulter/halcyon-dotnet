import { Outlet, useLocation } from 'react-router';
import { ErrorBoundary } from 'react-error-boundary';
import { Header } from '@/components/header';
import { Footer } from '@/components/footer';
import { ErrorPage } from '@/error-page';

export function Layout() {
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
