import type { RouteObject } from 'react-router';
import { LoginPage } from '@/features/account/login/login-page';
import { RegisterPage } from '@/features/account/register/register-page';
import { ForgotPasswordPage } from '@/features/account/forgot-password/forgot-password-page';
import { ResetPasswordPage } from '@/features/account/reset-password/reset-password-page';

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
