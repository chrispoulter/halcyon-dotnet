import { useMutation } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

type LoginRequest = {
    emailAddress: string;
    password: string;
};

type LoginResponse = {
    requiresTwoFactor: boolean;
    accessToken: string | null;
};

export const useLogin = () => {
    return useMutation({
        mutationFn: (request: LoginRequest) =>
            apiClient.post<LoginResponse>('/account/login', request),
    });
};
