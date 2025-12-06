import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type GenerateRecoveryCodesResponse = {
    id: string;
    recoveryCodes: string[];
};

export const useGenerateRecoveryCodes = () => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['recovery-codes'],
        queryFn: () =>
            apiClient
                .put('/profile/generate-recovery-codes', {
                    context: {
                        accessToken,
                    },
                })
                .json<GenerateRecoveryCodesResponse>(),
    });
};
