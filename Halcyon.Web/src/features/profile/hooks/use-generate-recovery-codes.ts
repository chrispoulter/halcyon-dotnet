import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type GenerateRecoveryCodesRequest = { version?: number };

type GenerateRecoveryCodesResponse = {
    id: string;
    recoveryCodes: string[];
};

export const useGenerateRecoveryCodes = (
    request: GenerateRecoveryCodesRequest
) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['recovery-codes'],
        queryFn: () =>
            apiClient.put<GenerateRecoveryCodesResponse>(
                '/profile/generate-recovery-codes',
                request,
                {
                    Authorization: `Bearer ${accessToken}`,
                }
            ),
    });
};
