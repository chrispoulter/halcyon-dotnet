import { Outlet } from 'react-router';
import { Header } from './header';
import { Footer } from './footer';

export function Layout() {
    return (
        <>
            <Header />
            <Outlet />
            <Footer />
        </>
    );
}
