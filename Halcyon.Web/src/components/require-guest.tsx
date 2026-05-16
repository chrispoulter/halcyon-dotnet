import { Navigate, Outlet } from 'react-router';
import { useAuth } from './auth-provider';

export function RequireGuest() {
    const { user } = useAuth();

    if (user) {
        return <Navigate to="/" />;
    }

    return <Outlet />;
}
