import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type SetupTwoFactorRequest = { version?: number };

type SetupTwoFactorResponse = {
    userId: string;
    secret: string;
    otpAuthUri: string;
};

export const useSetupTwoFactor = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: SetupTwoFactorRequest) =>
            apiClient.put<SetupTwoFactorResponse>(
                '/profile/setup-two-factor',
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
