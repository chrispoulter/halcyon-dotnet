import type { RouteObject } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { ProfilePage } from '@/features/profile/profile/profile-page';
import { UpdateProfilePage } from '@/features/profile/update-profile/update-profile-page';
import { ChangePasswordPage } from '@/features/profile/change-password/change-password-page';
import { SetupTwoFactorPage } from '@/features/profile/setup-two-factor/setup-two-factor-page';

export const profileRoutes: RouteObject[] = [
    {
        path: 'profile',
        Component: ProtectedRoute,
        children: [
            { index: true, Component: ProfilePage },
            { path: 'update-profile', Component: UpdateProfilePage },
            { path: 'change-password', Component: ChangePasswordPage },
            { path: 'setup-two-factor', Component: SetupTwoFactorPage },
        ],
    },
];
