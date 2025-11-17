import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

export type SetupTwoFactorResponse = {
    id: string;
    secret: string;
    otpauthUri: string;
    qrContent: string; // same as otpauthUri (server returns the URI)
};

export const useSetupTwoFactor = () => {
    const { accessToken } = useAuth();
    
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient.post<SetupTwoFactorResponse>(
                '/profile/2fa/setup',
                undefined,
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
