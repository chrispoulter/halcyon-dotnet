import type { RouteObject } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { CreateUserPage } from '@/features/users/create-user/create-user-page';
import { SearchUsersPage } from '@/features/users/search-users/search-users-page';
import { UpdateUserPage } from '@/features/users/update-user/update-user-page';
import { isUserAdministrator } from '@/lib/session';

export const usersRoutes: RouteObject[] = [
    {
        path: 'users',
        Component: () => <ProtectedRoute roles={isUserAdministrator} />,
        children: [
            { index: true, Component: SearchUsersPage },
            { path: 'create', Component: CreateUserPage },
            { path: ':id', Component: UpdateUserPage },
        ],
    },
];
