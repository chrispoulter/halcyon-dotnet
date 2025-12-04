import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

export type GetProfileResponse = {
    id: string;
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
};

export const useGetProfile = () => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['profile'],
        queryFn: ({ signal }) =>
            apiClient
                .get('profile', {
                    context: {
                        accessToken,
                    },
                    signal,
                })
                .json<GetProfileResponse>(),
    });
};
