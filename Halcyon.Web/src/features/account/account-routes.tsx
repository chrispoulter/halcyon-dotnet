import { Route } from 'react-router';
import { LoginPage } from '@/features/account/login/login-page';
import { RegisterPage } from '@/features/account/register/register-page';
import { ForgotPasswordPage } from '@/features/account/forgot-password/forgot-password-page';
import { ResetPasswordPage } from '@/features/account/reset-password/reset-password-page';

export const AccountRoutes = () => {
    return (
        <Route path="account">
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="forgot-password" element={<ForgotPasswordPage />} />
            <Route
                path="reset-password/:token"
                element={<ResetPasswordPage />}
            />
        </Route>
    );
};
