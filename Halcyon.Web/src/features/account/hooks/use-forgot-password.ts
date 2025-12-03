import { useMutation } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

type ForgotPasswordRequest = { emailAddress: string };

export const useForgotPassword = () =>
    useMutation({
        mutationFn: (request: ForgotPasswordRequest) =>
            apiClient.put('account/forgot-password', { json: request }).json(),
    });
