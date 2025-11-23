import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type SetupTwoFactorRequest = { version?: number };

type SetupTwoFactorResponse = {
    id: string;
    secret: string;
    otpAuthUri: string;
};

export const useSetupTwoFactor = (request: SetupTwoFactorRequest) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['twofactor'],
        queryFn: () =>
            apiClient.put<SetupTwoFactorResponse>('/profile/setup-two-factor', request, {
                Authorization: `Bearer ${accessToken}`,
            }),
    });
};
