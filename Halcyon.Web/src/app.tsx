import { Routes, Route } from 'react-router';
import { RootLayout } from '@/components/root-layout';
import { NotFoundPage } from '@/not-found-page';
import { HomePage } from '@/home-page';

import { accountRoutes } from '@/features/account/account-routes';
import { profileRoutes } from '@/features/profile/profile-routes';
import { usersRoutes } from '@/features/users/users-routes';

export default function App() {
    return (
        <Routes>
            <Route element={<RootLayout />}>
                <Route index element={<HomePage />} />
                {accountRoutes}
                {profileRoutes}
                {usersRoutes}
                <Route path="*" element={<NotFoundPage />} />
            </Route>
        </Routes>
    );
}
