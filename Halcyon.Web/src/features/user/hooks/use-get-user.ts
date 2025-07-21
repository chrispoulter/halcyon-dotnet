import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import type { GetUserResponse } from '@/features/user/user-types';
import { apiClient } from '@/lib/api-client';

export const useGetUser = (id: string) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['user', id],
        queryFn: ({ signal }) =>
            apiClient.get<GetUserResponse>(`/user/${id}`, signal, undefined, {
                Authorization: `Bearer ${accessToken}`,
            }),
    });
};
