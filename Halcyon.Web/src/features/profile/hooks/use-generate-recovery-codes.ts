import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type GenerateRecoveryCodesRequest = { version?: number };

type GenerateRecoveryCodesResponse = {
    userId: string;
    recoveryCodes: string[];
};

export const useGenerateRecoveryCodes = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: GenerateRecoveryCodesRequest) =>
            apiClient.put<GenerateRecoveryCodesResponse>(
                '/profile/generate-recovery-codes',
                request,
                {
                    Authorization: `Bearer ${accessToken}`,
                }
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
