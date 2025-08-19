import type { RouteObject } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { CreateUserPage } from '@/features/user/create-user/create-user-page';
import { SearchUsersPage } from '@/features/user/search-users/search-users-page';
import { UpdateUserPage } from '@/features/user/update-user/update-user-page';
import { isUserAdministrator } from '@/lib/session';

export const userRoutes: RouteObject[] = [
    {
        path: 'user',
        Component: () => <ProtectedRoute roles={isUserAdministrator} />,
        children: [
            { index: true, Component: SearchUsersPage },
            { path: 'create', Component: CreateUserPage },
            { path: ':id', Component: UpdateUserPage },
        ],
    },
];
