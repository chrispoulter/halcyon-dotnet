import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type SetupTwoFactorResponse = {
    id: string;
    secret: string;
    otpAuthUri: string;
};

export const useSetupTwoFactor = () => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['two-factor'],
        queryFn: () =>
            apiClient
                .put('/profile/setup-two-factor', {
                    context: {
                        accessToken,
                    },
                })
                .json<SetupTwoFactorResponse>(),
    });
};
