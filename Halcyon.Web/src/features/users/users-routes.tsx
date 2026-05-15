import type { RouteObject } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { isUserAdministrator } from '@/lib/session';
import { CreateUserPage } from './create-user/create-user-page';
import { SearchUsersPage } from './search-users/search-users-page';
import { UpdateUserPage } from './update-user/update-user-page';

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
