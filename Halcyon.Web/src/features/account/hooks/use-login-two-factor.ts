import { useMutation } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

export type LoginTwoFactorRequest = {
    emailAddress: string;
    password: string;
    code?: string;
    recoveryCode?: string;
};

export type LoginTwoFactorResponse = {
    accessToken: string;
};

export const useLoginTwoFactor = () => {
    return useMutation({
        mutationFn: (request: LoginTwoFactorRequest) =>
            apiClient.post<LoginTwoFactorResponse>(
                '/account/login/2fa',
                request
            ),
    });
};
