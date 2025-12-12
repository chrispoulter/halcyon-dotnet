import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type VerifyTwoFactorRequest = { verificationCode: string };

type VerifyTwoFactorResponse = {
    id: string;
    recoveryCodes: string[];
};

export const useVerifyTwoFactor = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: VerifyTwoFactorRequest) =>
            apiClient
                .put('/profile/verify-two-factor', {
                    json: request,
                    context: {
                        accessToken,
                    },
                })
                .json<VerifyTwoFactorResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['two-factor'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', data.id] });
        },
    });
};
