import { Route } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { ProfilePage } from '@/features/profile/profile/profile-page';
import { UpdateProfilePage } from '@/features/profile/update-profile/update-profile-page';
import { ChangePasswordPage } from '@/features/profile/change-password/change-password-page';

export const ProfileRoutes = () => {
    return (
        <Route path="profile" element={<ProtectedRoute />}>
            <Route index element={<ProfilePage />} />
            <Route path="update-profile" element={<UpdateProfilePage />} />
            <Route path="change-password" element={<ChangePasswordPage />} />
        </Route>
    );
};
