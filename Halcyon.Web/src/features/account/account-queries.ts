import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';
import { profileKeys } from '../profile/profile-queries';
import { userKeys } from '../users/users-queries';

type LoginRequest = {
    emailAddress: string;
    password: string;
};

type LoginResponse = {
    accessToken: string;
};

export const useLogin = () =>
    useMutation({
        mutationFn: (request: LoginRequest) =>
            apiClient
                .post('account/login', { json: request })
                .json<LoginResponse>(),
    });

type RegisterRequest = {
    emailAddress: string;
    password: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
};

type RegisterResponse = {
    id: string;
};

export const useRegister = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: RegisterRequest) =>
            apiClient
                .post('account/register', { json: request })
                .json<RegisterResponse>(),
        onSuccess: () =>
            queryClient.invalidateQueries({ queryKey: userKeys.all }),
    });
};

type ForgotPasswordRequest = { emailAddress: string };

export const useForgotPassword = () =>
    useMutation({
        mutationFn: (request: ForgotPasswordRequest) =>
            apiClient.put('account/forgot-password', { json: request }).json(),
    });

type ResetPasswordRequest = {
    token: string;
    emailAddress: string;
    newPassword: string;
};

type ResetPasswordResponse = {
    id: string;
};

export const useResetPassword = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: ResetPasswordRequest) =>
            apiClient
                .put('account/reset-password', { json: request })
                .json<ResetPasswordResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: profileKeys.all });
            queryClient.invalidateQueries({ queryKey: userKeys.all });
            queryClient.invalidateQueries({
                queryKey: userKeys.detail(data.id),
            });
        },
    });
};
