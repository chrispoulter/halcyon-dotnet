import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

type ResetPasswordRequest = {
    token: string;
    emailAddress: string;
    newPassword: string;
};

type ResetPasswordResponse = {
    userId: string;
};

export const useResetPassword = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: ResetPasswordRequest) =>
            apiClient.put<ResetPasswordResponse>(
                '/account/reset-password',
                request
            ),
        onSuccess: (response) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({
                queryKey: ['user', response.userId],
            });
        },
    });
};
