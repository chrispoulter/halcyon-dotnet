import { Route } from 'react-router';
import { ProtectedRoute } from '@/components/protected-route';
import { isUserAdministrator } from '@/lib/session';
import { CreateUserPage } from './create-user/create-user-page';
import { SearchUsersPage } from './search-users/search-users-page';
import { UpdateUserPage } from './update-user/update-user-page';

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
