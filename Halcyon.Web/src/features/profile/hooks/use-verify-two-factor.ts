import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

export type VerifyTwoFactorRequest = { code: string };

export type VerifyTwoFactorResponse = {
    id: string;
    enabled: boolean;
    recoveryCodes: string[];
};

export const useVerifyTwoFactor = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: VerifyTwoFactorRequest) =>
            apiClient.post<VerifyTwoFactorResponse>(
                '/profile/2fa/verify',
                request,
                {
                    Authorization: `Bearer ${accessToken}`,
                }
            ),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', data.id] });
        },
    });
};
