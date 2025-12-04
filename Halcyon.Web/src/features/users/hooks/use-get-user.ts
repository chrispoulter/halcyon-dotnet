import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';
import type { Role } from '@/lib/session';

export type GetUserResponse = {
    id: string;
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    isTwoFactorEnabled?: boolean;
    isLockedOut?: boolean;
    roles?: Role[];
};

export const useGetUser = (id: string) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['user', id],
        queryFn: ({ signal }) =>
            apiClient
                .get(`users/${id}`, {
                    context: {
                        accessToken,
                    },
                    signal,
                })
                .json<GetUserResponse>(),
    });
};
