import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type DisableTwoFactorRequest = { version?: number };

type DisableTwoFactorResponse = { id: string };

export const useDisableTwoFactor = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: DisableTwoFactorRequest) =>
            apiClient.put<DisableTwoFactorResponse>(
                '/profile/disable-two-factor',
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
