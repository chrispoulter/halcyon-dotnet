import { Route } from 'react-router';
import { RequireAuth } from '@/components/require-auth';
import { ProfilePage } from './profile/profile-page';
import { UpdateProfilePage } from './update-profile/update-profile-page';
import { ChangePasswordPage } from './change-password/change-password-page';

export const profileRoutes = (
    <Route path="profile" element={<RequireAuth />}>
        <Route index element={<ProfilePage />} />
        <Route path="update-profile" element={<UpdateProfilePage />} />
        <Route path="change-password" element={<ChangePasswordPage />} />
    </Route>
);
