import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type GenerateRecoveryCodesResponse = {
    id: string;
    recoveryCodes: string[];
};

export const useGenerateRecoveryCodes = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient
                .put('/profile/generate-recovery-codes', {
                    context: {
                        accessToken,
                    },
                })
                .json<GenerateRecoveryCodesResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', data.id] });
        },
    });
};
