import { Route } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { CreateUserPage } from '@/features/users/create-user/create-user-page';
import { SearchUsersPage } from '@/features/users/search-users/search-users-page';
import { UpdateUserPage } from '@/features/users/update-user/update-user-page';
import { isUserAdministrator } from '@/lib/session';

export const UsersRoutes = () => {
    return (
        <Route
            path="users"
            element={<ProtectedRoute roles={isUserAdministrator} />}
        >
            <Route index element={<SearchUsersPage />} />
            <Route path="create" element={<CreateUserPage />} />
            <Route path=":id" element={<UpdateUserPage />} />
        </Route>
    );
};
