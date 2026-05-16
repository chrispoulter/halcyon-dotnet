import { Route } from 'react-router';
import { RequireAuth } from '@/components/require-auth';
import { isUserAdministrator } from '@/lib/session';
import { CreateUserPage } from './create-user/create-user-page';
import { SearchUsersPage } from './search-users/search-users-page';
import { UpdateUserPage } from './update-user/update-user-page';

export const usersRoutes = (
    <Route path="users" element={<RequireAuth roles={isUserAdministrator} />}>
        <Route index element={<SearchUsersPage />} />
        <Route path="create" element={<CreateUserPage />} />
        <Route path=":id" element={<UpdateUserPage />} />
    </Route>
);
