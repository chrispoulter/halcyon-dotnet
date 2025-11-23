import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type VerifyTwoFactorRequest = { code: string; version?: number };

type VerifyTwoFactorResponse = {
    id: string;
    recoveryCodes: string[];
};

export const useVerifyTwoFactor = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: VerifyTwoFactorRequest) =>
            apiClient.put<VerifyTwoFactorResponse>(
                '/profile/verify-two-factor',
                request,
                {
                    Authorization: `Bearer ${accessToken}`,
                }
            ),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({
                queryKey: ['user', data.id],
            });
        },
    });
};
