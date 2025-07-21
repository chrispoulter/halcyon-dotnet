import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import type { GetProfileResponse } from '@/features/profile/profile-types';
import { apiClient } from '@/lib/api-client';

export const useGetProfile = () => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['profile'],
        queryFn: ({ signal }) =>
            apiClient.get<GetProfileResponse>('/profile', signal, undefined, {
                Authorization: `Bearer ${accessToken}`,
            }),
    });
};
