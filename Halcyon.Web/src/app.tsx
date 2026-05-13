import { Routes, Route } from 'react-router';
import { Layout } from '@/components/layout';
import { NotFoundPage } from '@/not-found-page';
import { HomePage } from '@/home-page';

import { AccountRoutes } from '@/features/account/account-routes';
import { ProfileRoutes } from '@/features/profile/profile-routes';
import { UsersRoutes } from '@/features/users/users-routes';

export default function App() {
    return (
        <Routes>
            <Route element={<Layout />}>
                <Route index element={<HomePage />} />
                <AccountRoutes />
                <ProfileRoutes />
                <UsersRoutes />
                <Route path="*" element={<NotFoundPage />} />
            </Route>
        </Routes>
    );
}
