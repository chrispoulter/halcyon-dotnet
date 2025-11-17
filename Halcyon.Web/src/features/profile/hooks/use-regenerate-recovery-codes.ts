import { useMutation } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

export type RegenerateRecoveryCodesResponse = {
    userId: string;
    recoveryCodes: string[];
};

export const useRegenerateRecoveryCodes = () => {
    const { accessToken } = useAuth();

    return useMutation({
        mutationFn: () =>
            apiClient.post<RegenerateRecoveryCodesResponse>(
                '/profile/2fa/recovery/regenerate',
                undefined,
                { Authorization: `Bearer ${accessToken}` }
            ),
    });
};
