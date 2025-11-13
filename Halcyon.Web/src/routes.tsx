import type { RouteObject } from 'react-router';
import { Layout } from '@/components/layout';
import { HomePage } from '@/home-page';
import { NotFoundPage } from '@/not-found-page';
import { ErrorPage } from '@/error-page';

import { accountRoutes } from '@/features/account/account-routes';
import { profileRoutes } from '@/features/profile/profile-routes';
import { usersRoutes } from '@/features/users/users-routes';

export const routes: RouteObject[] = [
    {
        Component: Layout,
        children: [
            {
                ErrorBoundary: ErrorPage,
                children: [
                    {
                        index: true,
                        Component: HomePage,
                    },
                    ...accountRoutes,
                    ...profileRoutes,
                    ...usersRoutes,
                    {
                        path: '*',
                        Component: NotFoundPage,
                    },
                ],
            },
        ],
    },
];
