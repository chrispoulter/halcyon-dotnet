import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

export type DisableTwoFactorResponse = { id: string };

export const useDisableTwoFactor = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient.post<DisableTwoFactorResponse>(
                '/profile/2fa/disable',
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
