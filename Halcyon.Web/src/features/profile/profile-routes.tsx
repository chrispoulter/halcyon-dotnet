import type { RouteObject } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { ProfilePage } from './profile/profile-page';
import { UpdateProfilePage } from './update-profile/update-profile-page';
import { ChangePasswordPage } from './change-password/change-password-page';

export const profileRoutes: RouteObject[] = [
    {
        path: 'profile',
        Component: ProtectedRoute,
        children: [
            { index: true, Component: ProfilePage },
            { path: 'update-profile', Component: UpdateProfilePage },
            { path: 'change-password', Component: ChangePasswordPage },
        ],
    },
];
