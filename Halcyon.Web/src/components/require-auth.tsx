import { Navigate, Outlet } from 'react-router';
import type { Role } from '@/lib/session';
import { ForbiddenPage } from '@/forbidden-page';
import { useAuth } from './auth-provider';

type RequireAuthProps = {
    roles?: Role[];
};

export function RequireAuth({ roles }: RequireAuthProps) {
    const { user } = useAuth();

    if (!user) {
        return <Navigate to="/account/login" />;
    }

    if (roles && !roles.some((value) => user.roles?.includes(value))) {
        return <ForbiddenPage />;
    }

    return <Outlet />;
}
