import type { RouteObject } from 'react-router';
import { LoginPage } from './login/login-page';
import { RegisterPage } from './register/register-page';
import { ForgotPasswordPage } from './forgot-password/forgot-password-page';
import { ResetPasswordPage } from './reset-password/reset-password-page';

export const accountRoutes: RouteObject[] = [
    {
        path: 'account',
        children: [
            { path: 'login', Component: LoginPage },
            { path: 'register', Component: RegisterPage },
            { path: 'forgot-password', Component: ForgotPasswordPage },
            { path: 'reset-password/:token', Component: ResetPasswordPage },
        ],
    },
];
