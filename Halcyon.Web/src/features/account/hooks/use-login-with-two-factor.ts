import { useMutation } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

type LoginWithTwoFactorRequest = {
    emailAddress: string;
    password: string;
    code: string;
};

type LoginWithTwoFactorResponse = {
    accessToken: string;
};

export const useLoginWithTwoFactor = () => {
    return useMutation({
        mutationFn: (request: LoginWithTwoFactorRequest) =>
            apiClient.post<LoginWithTwoFactorResponse>(
                '/account/login-two-factor',
                request
            ),
    });
};
