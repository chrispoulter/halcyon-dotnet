import { useMutation } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

type LoginWithRecoveryCodeRequest = {
    emailAddress: string;
    password: string;
    recoveryCode: string;
};

type LoginWithRecoveryCodeResponse = {
    accessToken: string;
};

export const useLoginWithRecoveryCode = () => {
    return useMutation({
        mutationFn: (request: LoginWithRecoveryCodeRequest) =>
            apiClient
                .post('/account/login-recovery-code', { json: request })
                .json<LoginWithRecoveryCodeResponse>(),
    });
};
